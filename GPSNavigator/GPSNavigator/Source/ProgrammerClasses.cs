using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace GPSNavigator.Source
{

    public class Programmer
    {
        public FileStream MCSFile;
        public IntelHexParser parser;
        public FlashMemory localMemory;
        public Form1 Parentform;
        public ControlPanel Controlpanelform;
        int baseAddress = 0;

        public Programmer(FileStream filetoprogram,Form1 Parent, ControlPanel controlpanelform)
        {
            MCSFile = filetoprogram;
            localMemory = new FlashMemory();
            parser = new IntelHexParser();
            Controlpanelform = controlpanelform;
            Parentform = Parent;
        }

        public byte[] formatString(byte[] str)
        {
            byte[] output = new byte[1];
            for (int i = 0; i < str.Length; i++)
                if (str[i] == '\r' || str[i] == '\n')
                {
                    str[i] = 0;
                    output = new byte[i];
                    for (int j = 0; j < i;j++)
                        output[j] = str[j];
                    break;
                }
            return output;
        }


        public void StartProgram(int baudrate, bool erase, bool verify)
        {
            Parentform.Programming_Mode = true;
            if (!erase || verify)
            {
                Controlpanelform.SetStatusText("Reading File...");
                MCSFile.Position = 0;
                char[] buffer = new char[100];
                char[] cbuffer = new char[100];
                double prog;
                while (MCSFile.Position < MCSFile.Length)
                {
                    prog = (double)MCSFile.Position / MCSFile.Length * 100;
                    Parentform.ProgressbarChangeValue((int)prog);
                    var len = 0;
                    while (true)
                    {
                        var readbyte = MCSFile.ReadByte();
                        if (readbyte == (int)'\r' || readbyte == (int)'\n' || MCSFile.Position >= MCSFile.Length)
                            break;
                        else
                        {
                            buffer[len] = (char)readbyte;
                            len++;
                        }

                    }

                    cbuffer = new char[len];
                    Array.Copy(buffer, cbuffer, len);

                    if (parser.parseRecord(cbuffer))
                    {
                        IntelHexRecord record = parser.getRecord();
                        switch (record.type)
                        {
                            case IntelHexRecordType.INTEL_HEX_RECORD_TYPE_EXTENDED_LINEAR_ADDRESS:
                                for (int i = 0; i < record.byteCount; i++)
                                    baseAddress = (baseAddress << 8) | record.data[i];
                                for (int i = record.byteCount; i < 4; i++)
                                    baseAddress <<= 8;
                                break;
                            case IntelHexRecordType.INTEL_HEX_RECORD_TYPE_DATA:
                                for (int i = 0; i < record.byteCount; i++)
                                    localMemory.setData(baseAddress + record.address + i, record.data[i]);
                                break;
                            case IntelHexRecordType.INTEL_HEX_RECORD_TYPE_END_OF_FILE:
                                break;
                        }
                    }
                    else
                    {
                        Controlpanelform.SetStatusText("Error in File Reading");
                        return;
                    }
                }
            }

            RemoteFlashInterface remotemem = new RemoteFlashInterface(new char[1],Parentform.serialPort1.BaudRate,Parentform);
            Controlpanelform.SetStatusText("Opening Connection");
            if (!remotemem.openConnection(baudrate))
            {
                Controlpanelform.SetStatusText("Error in Connection");
                return;
            }
            else
            {

                if (erase)
                {
                    Controlpanelform.SetStatusText("Erasing");
                    if (Erase(remotemem))
                        Controlpanelform.SetStatusText("Erasing Successful");
                    else
                        Controlpanelform.SetStatusText("Erasing Error");

                }
                else
                {
                    if (verify)
                    {
                        Controlpanelform.SetStatusText("Verifying");
                        if (Verify(remotemem))
                            Controlpanelform.SetStatusText("100% Matching");
                        else
                            Controlpanelform.SetStatusText("Not Matching");
                    }
                    else
                    {
                        Controlpanelform.SetStatusText("Programming Chip");
                        if (Synchronise(remotemem))
                            Controlpanelform.SetStatusText("Programming Successful");
                        else
                            Controlpanelform.SetStatusText("Programming Error");

                    }
                }

                remotemem.closeConnection();
            }
            Parentform.Programming_Mode = false;
            remotemem = null;
            localMemory = null;
        }

        private bool Erase(RemoteFlashInterface Interface)
        {
            return Interface.fullErase();
        }

        private bool Verify(RemoteFlashInterface Interface)
        {
            return Interface.verifyChecksum(localMemory);
        }

        private bool Synchronise(RemoteFlashInterface Interface)
        {
            return Interface.synchronize(localMemory);
        }
    }

    public class FlashSector
    {

        public char[] data;

        public FlashSector()
        {
            data = new char[Functions.FLASH_SECTOR_SIZE];
        }

        public FlashSector(FlashSector f)
        {
            data = new char[Functions.FLASH_SECTOR_SIZE];
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                data[i] = f.data[i];
        }
	    public uint checksum()
        {
            uint output = Functions.crc32b(data, Functions.FLASH_SECTOR_SIZE, (uint)0);
            return output;
        }


    }

    public class IntelHexRecord
    {
	    public char byteCount;
	    public int address;
	    public IntelHexRecordType type;
	    public char[] data;
	    public char checksum;
	    public IntelHexRecord()
        {
            data = new char[Functions.INTEL_HEX_MAX_DATA_LENGTH];
            reset();
        }
        public void reset()
        {
            byteCount = (char)0;
            address = 0;
            checksum = (char)0;
            type = IntelHexRecordType.INTEL_HEX_RECORD_TYPE_DATA;
            for (int i = 0; i < data.Length; i++)
                data[i] = (char)(0xFF);
        }
    }

    public class IntelHexParser
    {
        private IntelHexRecord record;
        public IntelHexParser()
        {
            record = new IntelHexRecord();
            record.reset();
        }


        private char[] extractBytes(char[] record, int startOffset, int length)
        {
            char[] dest = new char[length];
            for (int i = 0; i < length; i++)
                dest[i] = record[startOffset + i];

            return dest;
        }
        private char extractByteCount(char[] record)
        {
            char[] byteCountChar = extractBytes(record, Functions.INTEL_HEX_BYTE_COUNT_OFFSET, Functions.INTEL_HEX_BYTE_COUNT_LENGTH);
	        return (char) Functions.hexToDecimal(byteCountChar,0, Functions.INTEL_HEX_BYTE_COUNT_LENGTH);
        }

        private int extractAddress(char[] record)
        {
            char[] addressChar = extractBytes(record, Functions.INTEL_HEX_ADDRESS_OFFSET, Functions.INTEL_HEX_ADDRESS_LENGTH);
	        return Functions.hexToDecimal(addressChar,0, Functions.INTEL_HEX_ADDRESS_LENGTH);
        }

        private IntelHexRecordType extractRecordType(char[] record)
        {
            	char[] recordTypeChar = extractBytes(record,  Functions.INTEL_HEX_RECORD_TYPE_OFFSET, Functions.INTEL_HEX_RECORD_TYPE_LENGTH);
	            return (IntelHexRecordType) Functions.hexToDecimal(recordTypeChar,0, Functions.INTEL_HEX_RECORD_TYPE_LENGTH);
        }

        private char[] extractData(char[] record, ref char dataLength)
        {
            char byteCount = extractByteCount(record);
	        char[] dataChar = extractBytes(record, Functions.INTEL_HEX_DATA_OFFSET, byteCount*2);
	        dataLength = (char) byteCount;
            char[] data = new char[byteCount];
	        for (int i=0; i<byteCount; i++)
		        data[i] = (char) Functions.hexToDecimal(dataChar,i*2,2);

            return data;
        }
        private char extractChecksum(char[] record)
        {
	            char byteCount = extractByteCount(record);
                char[] checksumChar = extractBytes(record, Functions.INTEL_HEX_DATA_OFFSET + byteCount * 2, Functions.INTEL_HEX_CHECKSUM_LENGTH);
	            return (char) Functions.hexToDecimal(checksumChar,0, Functions.INTEL_HEX_CHECKSUM_LENGTH);
        }

        public bool parseRecord(char[] record)
        {
            if (record.Length < Functions.INTEL_HEX_MIN_RECORD_LENGTH)
		        return false;

	        if (record[0] != Functions.INTEL_HEX_START_CODE)
		        return false;

	        char dataLen = extractByteCount(record);

	        char checksum = (char)0;
	        for (int i=0; i<(record.Length-Functions.INTEL_HEX_START_CODE_LENGTH)/2;i++)
	        {
                char Byte = (char)Functions.hexToDecimal(record, 2 * i + Functions.INTEL_HEX_START_CODE_LENGTH, 2);
			        checksum += Byte;
	        }
	        if (checksum == (char)1)
		        return false;

            this.record.address = extractAddress(record);
            this.record.type = extractRecordType(record);
            this.record.data = extractData(record, ref this.record.byteCount);
	        return true;
        }
        public IntelHexRecord getRecord()
        {
            return record;
        }

        
    }

    public class FlashMemory
    {
        public FlashSector[] sector;
        public FlashSector EmptySector;
        public void setData(int address, char data)
        {
            int sectorNumber = address / Functions.FLASH_SECTOR_SIZE;
            int addressOffset = address % Functions.FLASH_SECTOR_SIZE;
            try
            {
                sector[sectorNumber].data[addressOffset] = data;
            }
            catch
            {
                sector[sectorNumber] = new FlashSector(EmptySector);
                sector[sectorNumber].data[addressOffset] = data;
            }
        }
        public FlashMemory()
        {
            sector = new FlashSector[Functions.FLASH_MEMORY_nSECTORS];
            EmptySector = new FlashSector();
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                EmptySector.data[i] = (char)0xFF;
        }
        public void erase()
        {
            for (int i = 0; i < Functions.FLASH_MEMORY_nSECTORS; i++)
                    sector[i] = new FlashSector(EmptySector);
        }
        public int checksum()
        {
            int checksum = 0;
            for (int i = 0; i < Functions.FLASH_MEMORY_nSECTORS; i++)
            {
                try
                {
                    checksum = Functions.crc32b(sector[i].data, Functions.FLASH_SECTOR_SIZE, checksum);
                }
                catch
                {
                    checksum = Functions.crc32b(EmptySector.data, Functions.FLASH_SECTOR_SIZE, checksum);
                }
            }
	        return checksum;
        }
    }

    public class RemoteFlashCommandPacket
    {
        private	RemoteFlashCommandCode commandCode;
	    private int sectorIndex;
	    private int dataLength;
	    private char[] data = new char[Functions.FLASH_SECTOR_SIZE];
        private char[] emptydata = new char[Functions.FLASH_SECTOR_SIZE];
        private char[] forchecksum;
        public char[] objectdescriber;

	    public int crc32;
        public void insertChecksum()
        {
            this.ToCharArray();
            crc32 = (int)Functions.crc32b(objectdescriber,objectdescriber.Length,0);
        }

        public RemoteFlashCommandPacket()
        {
            forchecksum = new char[12 + dataLength];
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                emptydata[i] = (char)0xFF;
            reset();
        }
        public void reset()
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_HELLO;
            sectorIndex = -1;
            dataLength = 0;
            data = emptydata;
            crc32 = 0;
        }
        public void setHelloCommand()
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_HELLO;
            sectorIndex = -1;
            dataLength = 0;
            data = emptydata;
            insertChecksum();
        }
        public void setDisconnectCommand()
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_DISCONNECT;
            sectorIndex = -1;
            dataLength = 0;
            data = emptydata;
            insertChecksum();
        }
        public void setGetChecksumCommand(int sectorIndex)
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_GET_CHECKSUM;
            this.sectorIndex = sectorIndex;
            dataLength = 0;
            data = emptydata;
            insertChecksum();
        }
        public void setGetChipChecksumCommand()
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_GET_CHIP_CHECKSUM;
            this.sectorIndex = -1;
            dataLength = 0;
            data = emptydata;
            insertChecksum();
        }
        public void setEraseSectorCommand(int sectorIndex)
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_ERASE_SECTOR;
            this.sectorIndex = sectorIndex;
            dataLength = 0;
            data = emptydata;
            insertChecksum();
        }
        public void setFullEraseCommand()
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_FULL_ERASE;
            sectorIndex = -1;
            dataLength = 0;
            data = emptydata;
            insertChecksum();
        }
        public void setProgramSectorCommand(int sectorIndex, char[] data)
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_PROGRAM_SECTOR;

            this.sectorIndex = sectorIndex;
            dataLength = Functions.FLASH_SECTOR_SIZE;
            this.data = data;
            insertChecksum();
        }
        public void setReadbackSectorCommand(int sectorIndex)
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_READBACK;
            this.sectorIndex = sectorIndex;
            dataLength = 0;
            data = emptydata;
            insertChecksum();
        }
        public void ToCharArray()
        {
            int length = 12 + dataLength;
            objectdescriber = new char[length];
            int code = (int)commandCode;
            for (int i = 0; i < 4; i++)
            {
                objectdescriber[i] = (char)(code % 256);
                code /= 256;
            }
            int index = 4;
            code = sectorIndex;
            for (int i = 0; i < 4; i++)
            {
                objectdescriber[index+i] = (char)(code & 0xFF);
                code >>= 8;
            }

            index += 4;
            code = dataLength;
            for (int i = 0; i < 4; i++)
            {
                objectdescriber[index + i] = (char)(code % 256);
                code /= 256;
            }
            index += 4;
            for (int i = 0; i < dataLength; i++)
                objectdescriber[index + i] = data[i];
        }
    };

    public class RemoteFlashInterface
    {

        private RemoteFlashCommandPacket commandPacket;
	    private RemoteFlashResponsePacket responsePacket;
	    private int programmingBaudrate;
	    public RemoteFlashConnectionStatus connectionStatus;
        private bool sendCommand()
        {
            char[] header =
	        {
			    Functions.REMOTE_FLASH_PACKET_HEADER0, Functions.REMOTE_FLASH_PACKET_HEADER1,
			    Functions.REMOTE_FLASH_PACKET_HEADER2, Functions.REMOTE_FLASH_PACKET_HEADER3
	        };
            Parentform.Serial1_Write(Functions.ChartoByte(header),0,4);
            Parentform.Serial1_Write(Functions.ChartoByte(commandPacket.objectdescriber),0,commandPacket.objectdescriber.Length);
            byte[] crc = new byte[4];
            int temp = commandPacket.crc32;
            for (int i = 0; i < 4; i++)
            {
                crc[i] = (byte)(temp & 0xFF);
                temp >>= 8;
            }
            Parentform.serialPort1.DiscardInBuffer();
            Parentform.Serial1_Write(crc,0,4);
            char[] line = new char[1];
            byte[] bline = new byte[4];

            while (true)
            {
                if (Parentform.serialPort1.BytesToRead > 12)
                    break;
            }
            int timeoutcounter = 200;
            while (true)
            {
                if (Parentform.serialPort1.ReadByte() == (byte)170)
                    break;
                timeoutcounter--;
                if (timeoutcounter <= 0)
                    return false;

            }
            bline[0] = (byte)170;
            Parentform.serialPort1.Read(bline, 1, 3);

            if (
                 bline[0] != (byte)Functions.REMOTE_FLASH_PACKET_HEADER0 ||
                 bline[1] != (byte)Functions.REMOTE_FLASH_PACKET_HEADER1 ||
                 bline[2] != (byte)Functions.REMOTE_FLASH_PACKET_HEADER2 ||
                 bline[3] != (byte)Functions.REMOTE_FLASH_PACKET_HEADER3
             )
                return false;
            responsePacket.data = new byte[Functions.FLASH_SECTOR_SIZE];
            responsePacket.reset();

            Parentform.serialPort1.Read(bline, 0, 4);
            responsePacket.responseCode = (RemoteFlashResponseCode)Functions.ByteToInt(bline);
            Parentform.serialPort1.Read(bline, 0, 4);
            responsePacket.dataLength = Functions.ByteToInt(bline);
            bline = new byte[responsePacket.dataLength];
            var datacount = Parentform.serialPort1.Read(bline, 0, responsePacket.dataLength);
            while (datacount < responsePacket.dataLength)
                datacount += Parentform.serialPort1.Read(bline, datacount, responsePacket.dataLength - datacount);
            responsePacket.data = bline;
            bline = new byte[4];
            var checksumcount = Parentform.serialPort1.Read(bline, 0, 4);
            while (checksumcount < 4)
                checksumcount += Parentform.serialPort1.Read(bline, checksumcount, 4 - checksumcount);
            responsePacket.crc32 = Functions.ByteToInt(bline);
            if (!responsePacket.verifyChecksum())
                return false;
            return true;
        }

        private bool sendReceiveCommandResponse(int serialTimoutRetries = 1)
        {
            if (connectionStatus == RemoteFlashConnectionStatus.REMOTE_FLASH_CONNECTION_STATUS_DISCONNECTED)
                return false;
            Parentform.serialPort1.DiscardInBuffer();
            //serial->clearReceiveBuffer();
            int remainingRetryCount = Functions.REMOTE_FLASH_RETRY_COUNT;
            while (remainingRetryCount > 0)
            {
                //sendCommand();
                if (sendCommand())//receiveResponse(serialTimoutRetries))
                    return true;
                remainingRetryCount--;
            }
            return false;
        }
        private int getSectorChecksum(int sectorIndex)
        {
            int checksum = -1;
            commandPacket.setGetChecksumCommand(sectorIndex);
	        if (!sendReceiveCommandResponse())
		        return -1;
	        if (responsePacket.responseCode != RemoteFlashResponseCode.REMOTE_FLASH_RESPONSE_CODE_SECTOR_CHECKSUM)
		        return -1;

	        if (responsePacket.dataLength != 4)
		        return -1;

	        for (int i=0; i<4; i++)
		        checksum = (checksum<<8) | (int)responsePacket.data[i];
	        return checksum;
        }
        private int getChipChecksum()
        {
            int checksum = -1;
            	commandPacket.setGetChipChecksumCommand();
	        if (!sendReceiveCommandResponse())
		        return -1;
	        if (responsePacket.responseCode !=  RemoteFlashResponseCode.REMOTE_FLASH_RESPONSE_CODE_CHIP_CHECKSUM)
		        return -1;

	        if (responsePacket.dataLength != 4)
		        return -1;

	        int temp = 0;
	        for (int i=0; i<4; i++)
		        temp = (temp<<8) | (char)responsePacket.data[i];

	        checksum = temp;
	        return checksum;
        }
        private bool eraseSector(int sectorIndex)
        {
            commandPacket.setEraseSectorCommand(sectorIndex);
            if (!sendReceiveCommandResponse())
                return false;
            if (responsePacket.responseCode != RemoteFlashResponseCode.REMOTE_FLASH_RESPONSE_CODE_ACK_ERASE)
                return false;
            return true;
        }
        private bool programSector(int sectorIndex, char[] data)
        {
            commandPacket.setProgramSectorCommand(sectorIndex, data);
            if (!sendReceiveCommandResponse())
            {
                //Parentform.f.Close();
                //Parentform.serialin.Close();
                return false;
            }
            if (responsePacket.responseCode != RemoteFlashResponseCode.REMOTE_FLASH_RESPONSE_CODE_ACK_PROGRAM)
                return false;
            return true;
        }

        public Form1 Parentform;
        public RemoteFlashInterface(char[] serialDeviceFile, int baudRate,Form1 Parent)
        {
            Parentform = Parent;
            programmingBaudrate = baudRate;
            connectionStatus = RemoteFlashConnectionStatus.REMOTE_FLASH_CONNECTION_STATUS_DISCONNECTED;
            commandPacket = new RemoteFlashCommandPacket();
            responsePacket = new RemoteFlashResponsePacket();
        }

        public bool openConnection(int previousBaudrate)
        {
	        char[] baudrateBytes = new char[3];
	        baudrateBytes[0] = (char)((programmingBaudrate>>16) & 0xff);
	        baudrateBytes[1] = (char)((programmingBaudrate>>8) & 0xff);
	        baudrateBytes[2] = (char)((programmingBaudrate) & 0xff);

	        char[] enterProgrammingModeCommand =
	        {
			        '$', 'C', 'M', 'D',										// Header
			        Functions.REMOTE_FLASH_PROGRAMMING_MODE_COMMAND,					// Command
			        baudrateBytes[0], baudrateBytes[1], baudrateBytes[2],	// baud rate
			        (char)0														// checksum
	        };
	        for (int i=0; i<8; i++)
		        enterProgrammingModeCommand[8] ^= enterProgrammingModeCommand[i];
            byte[] bytemsg = Functions.ChartoByte(enterProgrammingModeCommand);
            Parentform.Serial1_Write(bytemsg,0,9);

	        commandPacket.setHelloCommand();
	        char[] header =
	        {
			        Functions.REMOTE_FLASH_PACKET_HEADER0, Functions.REMOTE_FLASH_PACKET_HEADER1,
			        Functions.REMOTE_FLASH_PACKET_HEADER2, Functions.REMOTE_FLASH_PACKET_HEADER3
	        };
            var tempint = Functions.ChartoByte(header);
            Parentform.Serial1_Write(tempint,0,4);
            tempint = Functions.ChartoByte(commandPacket.objectdescriber);
            Parentform.Serial1_Write(tempint,0,commandPacket.objectdescriber.Length);
            byte[] crc = new byte[4];
            int temp = commandPacket.crc32;
            for (int i = 0; i<4;i++)
            {
                crc[i] = (byte)(temp & 0xFF);
                temp>>=8;
            }
            Parentform.serialPort1.DiscardInBuffer();
            Parentform.Serial1_Write(crc,0,4);
            Thread.Sleep(10);
            Parentform.serialPort1.Encoding = Encoding.Default;
            byte[] tempb = new byte[4];
            byte[] bline = new byte[16];

            int timeoutcounter = 200;
            while (true)
            {
                if (Parentform.serialPort1.BytesToRead > 15)
                    break;
                timeoutcounter--;
                if (timeoutcounter <= 0)
                    return false;
            }
            timeoutcounter = 200;
            while (true)
            {
                if (Parentform.serialPort1.ReadByte() == (byte)170)
                    break;
                timeoutcounter--;
                if (timeoutcounter <= 0)
                    return false;              
            }
            bline[0] = (byte)170;
            int read = Parentform.serialPort1.Read(bline,1, 15);
            while (read < 15)            
                read += Parentform.serialPort1.Read(bline, 1 + read, 15 - read);            

            if (
                 bline[0] != (byte)Functions.REMOTE_FLASH_PACKET_HEADER0 ||
                 bline[1] != (byte)Functions.REMOTE_FLASH_PACKET_HEADER1 ||
                 bline[2] != (byte)Functions.REMOTE_FLASH_PACKET_HEADER2 ||
                 bline[3] != (byte)Functions.REMOTE_FLASH_PACKET_HEADER3
             )
                return false;

            responsePacket.reset();

            Array.Copy(bline, 4, tempb, 0, 4);
            responsePacket.responseCode = (RemoteFlashResponseCode)Functions.ByteToInt(tempb);
            Array.Copy(bline, 8, tempb, 0, 4);
            responsePacket.dataLength = Functions.ByteToInt(tempb);
            Array.Copy(bline, 12, tempb, 0, 4);
            responsePacket.crc32 = Functions.ByteToInt(tempb);

            if (!responsePacket.verifyChecksum())
            {
                return false;
            }

	        if (responsePacket.responseCode != RemoteFlashResponseCode.REMOTE_FLASH_RESPONSE_CODE_ACK_HELLO)
		        return false;

	        connectionStatus = RemoteFlashConnectionStatus.REMOTE_FLASH_CONNECTION_STATUS_ESTABLISHED;
	        return true;
        }

        public bool closeConnection()
        {
            bool returnValue = true;
            if (connectionStatus == RemoteFlashConnectionStatus.REMOTE_FLASH_CONNECTION_STATUS_ESTABLISHED)
            {
                commandPacket.setDisconnectCommand();
                if (!sendReceiveCommandResponse())
                    returnValue = false;
                else if (responsePacket.responseCode != RemoteFlashResponseCode.REMOTE_FLASH_RESPONSE_CODE_ACK_DISCONNECT)
                    returnValue = false;
            }
            connectionStatus = RemoteFlashConnectionStatus.REMOTE_FLASH_CONNECTION_STATUS_DISCONNECTED;
            return returnValue;
        }

        public byte[] readbackSector(int sectorIndex)
        {
            byte[] data = new byte[Functions.FLASH_SECTOR_SIZE];
            commandPacket.setReadbackSectorCommand(sectorIndex);
            if (!sendReceiveCommandResponse())
                throw new Exception("Response Error");
            if (responsePacket.responseCode != RemoteFlashResponseCode.REMOTE_FLASH_RESPONSE_CODE_READBACK)
                throw new Exception("Response Code Error");

            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                data[i] = responsePacket.data[i];
            return data;
        }
        public bool synchronize(FlashMemory localMemory)
        {
	        int remoteChecksum;
	        int i,sectorchecksum;
            double progress;

	        for (i=0; i<Functions.FLASH_MEMORY_nSECTORS; i++)
	        {
       
                progress = (double)i / Functions.FLASH_MEMORY_nSECTORS;
                Parentform.ProgressbarChangeValue((int)(progress * 100));
                remoteChecksum = getSectorChecksum(i);
                if (remoteChecksum == -1)
                {
                    i--;
                    continue;
                }

                char[] toprogram;
                try
                {
                    sectorchecksum = (int)localMemory.sector[i].checksum();
                    toprogram = localMemory.sector[i].data;
                }
                catch
                {
                    sectorchecksum = -246126838;
                    toprogram = localMemory.EmptySector.data;
                }
		        if (remoteChecksum == sectorchecksum)
			        continue;

                var programresult = programSector(i, toprogram);
		        if (programresult)
			        continue;

		        i--;
	        }
	        remoteChecksum=getChipChecksum();
	        if (remoteChecksum == -1 )
		        return false;

            var localchecksum  = localMemory.checksum();
	        if (remoteChecksum != localchecksum)
		        return false;
	        return true;
        }
        public bool verifyChecksum(FlashMemory localMemory)
        {
	        int remoteChecksum;
            double progress;
            int localchecksum;
	        for (int i=0; i<Functions.FLASH_MEMORY_nSECTORS; i++)
	        {

                progress = (double)i / Functions.FLASH_MEMORY_nSECTORS;
                Parentform.ProgressbarChangeValue((int)(progress * 100));
                remoteChecksum = getSectorChecksum(i);
                if (remoteChecksum == -1)
                {
                    i--;
                    continue;
                }

                try
                {
                    localchecksum = (int)localMemory.sector[i].checksum();
                }
                catch
                {
                    localchecksum = -246126838;
                }


                if (remoteChecksum != localchecksum)
                    return false;
	        }
	        remoteChecksum=getChipChecksum();
	        if (remoteChecksum == -1)
		        return false;

            localchecksum = localMemory.checksum();
	        if (remoteChecksum != localchecksum)
		        return false;

	        return true;
        }
        public bool fullErase()
        {            
            commandPacket.setFullEraseCommand();
            if (!sendReceiveCommandResponse(Functions.REMOTE_FLASH_FULL_ERASE_TIMOUT_RETRIES))
                return false;
            if (responsePacket.responseCode != RemoteFlashResponseCode.REMOTE_FLASH_RESPONSE_CODE_ACK_FULL_ERASE)
                return false;
            return true;
        }
    };

    public class RemoteFlashResponsePacket
    {

        public RemoteFlashResponseCode responseCode;
        public int dataLength;
        public byte[] data;
        public int crc32;

        public RemoteFlashResponsePacket()
        {
            data = new byte[Functions.FLASH_SECTOR_SIZE];
            reset();
        }
        public void reset()
        {
            responseCode = RemoteFlashResponseCode.REMOTE_FLASH_RESPONSE_CODE_INVALID;
            dataLength = 0;
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                data[i] = (byte)0xFF;
            crc32 = 0;
        }
        public bool verifyChecksum()
        {
            byte[] tochecksum = new byte[8 + dataLength];
            int responsecodeint = (int)responseCode;
            for (int i = 0; i < 4; i++)
            {
                tochecksum[i] = (byte)(responsecodeint % 256);
                responsecodeint /= 256;
            }
            responsecodeint = dataLength;
            for (int i = 0; i < 4; i++)
            {
                tochecksum[i+4] = (byte)(responsecodeint % 256);
                responsecodeint /= 256;
            }
            for (int i = 0; i < dataLength; i++)
                tochecksum[i + 8] = data[i];
            int localChecksum = (int)Functions.crc32b(tochecksum, 8+dataLength,0);
	        if (localChecksum == crc32)
		        return true;
	        return false;
        }
    };

    public enum IntelHexRecordType
    {
        INTEL_HEX_RECORD_TYPE_DATA = 0,
        INTEL_HEX_RECORD_TYPE_END_OF_FILE = 1,
        INTEL_HEX_RECORD_TYPE_EXTENDED_LINEAR_ADDRESS = 4
    };
    public enum RemoteFlashCommandCode
    {
        REMOTE_FLASH_COMMAND_CODE_HELLO = 0,
        REMOTE_FLASH_COMMAND_CODE_GET_CHECKSUM,
        REMOTE_FLASH_COMMAND_CODE_GET_CHIP_CHECKSUM,
        REMOTE_FLASH_COMMAND_CODE_ERASE_SECTOR,
        REMOTE_FLASH_COMMAND_CODE_FULL_ERASE,
        REMOTE_FLASH_COMMAND_CODE_PROGRAM_SECTOR,
        REMOTE_FLASH_COMMAND_CODE_READBACK,
        REMOTE_FLASH_COMMAND_CODE_DISCONNECT,
        REMOTE_FLASH_COMMAND_CODE_FORCE_ENUM_SIZE = int.MaxValue
    };
    public enum RemoteFlashConnectionStatus
    {
        REMOTE_FLASH_CONNECTION_STATUS_DISCONNECTED = 0,
        REMOTE_FLASH_CONNECTION_STATUS_ESTABLISHED
    };

    public enum RemoteFlashResponseCode
    {
        REMOTE_FLASH_RESPONSE_CODE_ACK_HELLO = 0,
        REMOTE_FLASH_RESPONSE_CODE_SECTOR_CHECKSUM,
        REMOTE_FLASH_RESPONSE_CODE_CHIP_CHECKSUM,
        REMOTE_FLASH_RESPONSE_CODE_ACK_ERASE,
        REMOTE_FLASH_RESPONSE_CODE_ACK_FULL_ERASE,
        REMOTE_FLASH_RESPONSE_CODE_ACK_PROGRAM,
        REMOTE_FLASH_RESPONSE_CODE_READBACK,
        REMOTE_FLASH_RESPONSE_CODE_INVALID,
        REMOTE_FLASH_RESPONSE_CODE_ACK_DISCONNECT,
        REMOTE_FLASH_RESPONSE_CODE_FORCE_ENUM_SIZE = int.MaxValue
    };
}
