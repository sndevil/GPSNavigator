using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace GPSNavigator.Source
{

    public class Programmer
    {
        public FileStream MCSFile;
        public IntelHexParser parser;
        public FlashMemory localMemory;
        public Form1 Parentform;
        int baseAddress = 0;

        public Programmer(FileStream filetoprogram,Form1 Parent)
        {
            MCSFile = filetoprogram;
            localMemory = new FlashMemory();
            parser = new IntelHexParser();
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
            MCSFile.Position = 0;
            char[] buffer = new char[100];
            char[] cbuffer = new char[100];
            while (MCSFile.Position < MCSFile.Length)
            {
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

                //MCSFile.read
                //MCSFile.Read(buffer, 0, 100);
                //var bufferp = formatString(buffer);
                //cbuffer = new char[bufferp.Length];
                cbuffer = new char[len];
                Array.Copy(buffer, cbuffer, len);
                //for (int i = 0;i<buffer.Length;i++)
                //    cbuffer[i] = buffer[i];
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
                    throw new Exception("Failed to parse");
                }
            }

            RemoteFlashInterface remotemem = new RemoteFlashInterface(new char[1],Parentform.serialPort1.BaudRate,Parentform);
            if (!remotemem.openConnection(baudrate))
            {
                throw new Exception("Failed to connect to device");
            }

            if (erase)
                Erase(remotemem);
            if (verify)
                Verify(remotemem);
            else
                Synchronise(remotemem);

            remotemem.closeConnection();
        }

        private void Erase(RemoteFlashInterface Interface)
        {
            Interface.fullErase();
        }

        private void Verify(RemoteFlashInterface Interface)
        {
            Interface.verifyChecksum(localMemory);
        }

        private void Synchronise(RemoteFlashInterface Interface)
        {
            Interface.synchronize(localMemory);
        }
    }

    public class FlashSector
    {

        public char[] data;

        public FlashSector()
        {
            data = new char[Functions.FLASH_SECTOR_SIZE];
	        erase();
        }
    	public void erase()
        {
	        for (int i=0; i<Functions.FLASH_SECTOR_SIZE; i++)
		        data[i] = (char)0xFF;
        }
	    public int checksum()
        {
            return Functions.crc32b(data, Functions.FLASH_SECTOR_SIZE, 0);
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

            // CHECK THIS
	        //if (record.Length != (int)((int)dataLen*2+Functions.INTEL_HEX_CHECKSUM_LENGTH+Functions.INTEL_HEX_DATA_OFFSET))
		    //    return false;

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

        public void setData(int address, char data)
        {
            int sectorNumber = address / Functions.FLASH_SECTOR_SIZE;
            int addressOffset = address % Functions.FLASH_SECTOR_SIZE;
            sector[sectorNumber].data[addressOffset] = data;
        }
        public FlashMemory()
        {
            sector = new FlashSector[Functions.FLASH_MEMORY_nSECTORS];

            erase();
        }
        public void erase()
        {
            for (int i = 0; i < Functions.FLASH_MEMORY_nSECTORS; i++)
                try
                {
                    sector[i].erase();
                }
                catch
                {
                    sector[i] = new FlashSector();
                    sector[i].erase();
                }
        }
        public int checksum()
        {
            int checksum = 0;
	        for (int i=0; i<Functions.FLASH_MEMORY_nSECTORS; i++)
		        checksum = Functions.crc32b(sector[i].data, Functions.FLASH_SECTOR_SIZE, checksum);
	        return checksum;
        }
    }

    public class RemoteFlashCommandPacket
    {
        private	RemoteFlashCommandCode commandCode;
	    private int sectorIndex;
	    private int dataLength;
	    private char[] data = new char[Functions.FLASH_SECTOR_SIZE];
        private char[] forchecksum;
        public char[] objectdescriber;

	    public int crc32;
        public void insertChecksum()
        {
            this.ToCharArray();
            crc32 = Functions.crc32b(objectdescriber,objectdescriber.Length,0);
        }

        public RemoteFlashCommandPacket()
        {
            forchecksum = new char[12 + dataLength];
            reset();
        }
        public void reset()
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_HELLO;
            sectorIndex = -1;
            dataLength = 0;
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                data[i] = (char)0xFF;
            crc32 = 0;
        }
        public void setHelloCommand()
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_HELLO;
            sectorIndex = -1;
            dataLength = 0;
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                data[i] = (char)0xFF;
            insertChecksum();
        }
        public void setDisconnectCommand()
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_DISCONNECT;
            sectorIndex = -1;
            dataLength = 0;
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                data[i] = (char)0xFF;
            insertChecksum();
        }
        public void setGetChecksumCommand(int sectorIndex)
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_GET_CHECKSUM;
            this.sectorIndex = sectorIndex;
            dataLength = 0;
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                data[i] = (char)0xFF;
            insertChecksum();
        }
        public void setGetChipChecksumCommand()
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_GET_CHIP_CHECKSUM;
            this.sectorIndex = -1;
            dataLength = 0;
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                data[i] = (char)0xFF;
            insertChecksum();
        }
        public void setEraseSectorCommand(int sectorIndex)
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_ERASE_SECTOR;
            this.sectorIndex = sectorIndex;
            dataLength = 0;
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                data[i] = (char)0xFF;
            insertChecksum();
        }
        public void setFullEraseCommand()
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_FULL_ERASE;
            sectorIndex = -1;
            dataLength = 0;
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                data[i] = (char)0xFF;
            insertChecksum();
        }
        public void setProgramSectorCommand(int sectorIndex, char[] data)
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_PROGRAM_SECTOR;

            this.sectorIndex = sectorIndex;
            dataLength = Functions.FLASH_SECTOR_SIZE;
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                this.data[i] = data[i];
            insertChecksum();
        }
        public void setReadbackSectorCommand(int sectorIndex)
        {
            commandCode = RemoteFlashCommandCode.REMOTE_FLASH_COMMAND_CODE_READBACK;
            this.sectorIndex = sectorIndex;
            dataLength = 0;
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                data[i] = (char)0xFF;
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
            if (code >= 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    objectdescriber[index + i] = (char)(code % 256);
                    code /= 256;
                }
            }
            else
            {
                objectdescriber[4] = (char)255;
                objectdescriber[5] = (char)255;
                objectdescriber[6] = (char)255;
                objectdescriber[7] = (char)255;
            }
            index += 4;
            code = dataLength;
            for (int i = 3; i > 0; i--)
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
	    //private SerialPort* serial;
	    private int programmingBaudrate;
	    private RemoteFlashConnectionStatus connectionStatus;
        private bool sendCommand()
        {
            char[] header =
	        {
			    Functions.REMOTE_FLASH_PACKET_HEADER0, Functions.REMOTE_FLASH_PACKET_HEADER1,
			    Functions.REMOTE_FLASH_PACKET_HEADER2, Functions.REMOTE_FLASH_PACKET_HEADER3
	        };
            Parentform.Serial1_Write(Functions.ChartoByte(header),0,4);
            commandPacket.ToCharArray();
            //Parentform.f.Write(Functions.ChartoByte(commandPacket.objectdescriber), 0, commandPacket.objectdescriber.Length);
            Parentform.Serial1_Write(Functions.ChartoByte(commandPacket.objectdescriber),0,commandPacket.objectdescriber.Length);

            commandPacket.insertChecksum();
            byte[] crc = new byte[4];
            int temp = commandPacket.crc32;
            for (int i = 0; i < 4; i++)
            {
                crc[i] = (byte)(temp & 0xFF);
                temp >>= 8;
            }
            //Parentform.f.Write(crc, 0, 4);
            Parentform.Serial1_Write(crc,0,4);
            Parentform.f.Close();
            Thread.Sleep(10);
            Parentform.serialPort1.Encoding = Encoding.Default;
            char[] line = new char[1];
            while (true)
            {
                line = Parentform.serialPort1.ReadExisting().ToCharArray();
                if (line.Length > 0)
                    break;
            }
            if (
                 line[0] != Functions.REMOTE_FLASH_PACKET_HEADER0 ||
                 line[1] != Functions.REMOTE_FLASH_PACKET_HEADER1 ||
                 line[2] != Functions.REMOTE_FLASH_PACKET_HEADER2 ||
                 line[3] != Functions.REMOTE_FLASH_PACKET_HEADER3
             )
                return false;

            responsePacket.data = new char[Functions.FLASH_SECTOR_SIZE];
            responsePacket.reset();
            
            Array.Copy(line, 4, header, 0, 4);
            responsePacket.responseCode = (RemoteFlashResponseCode)Functions.CharToInt(header);
            Array.Copy(line, 8, header, 0, 4);
            responsePacket.dataLength = Functions.CharToInt(header);
            header = new char[responsePacket.dataLength];
            Array.Copy(line, 12, header, 0, responsePacket.dataLength);
            responsePacket.data = header;
            header = new char[4];
            Array.Copy(line, 12 + responsePacket.dataLength, header, 0, 4);
            responsePacket.crc32 = Functions.CharToInt(header);
            if (!responsePacket.verifyChecksum())
                return false;
            return true;
        }
        private bool receiveResponse(int serialTimoutRetries = 1)
        {
            char[] header = new char[Functions.REMOTE_FLASH_PACKET_HEADER_SIZE];
            //Parentform.f.Close();
            Parentform.serialPort1.Read(header,0,Functions.REMOTE_FLASH_PACKET_HEADER_SIZE);
	        if (
			        header[0] != Functions.REMOTE_FLASH_PACKET_HEADER0 ||
			        header[1] != Functions.REMOTE_FLASH_PACKET_HEADER1 ||
			        header[2] != Functions.REMOTE_FLASH_PACKET_HEADER2 ||
			        header[3] != Functions.REMOTE_FLASH_PACKET_HEADER3
		        )
		        return false;

	        responsePacket.reset();            
            Parentform.serialPort1.Read(header,0,4);
            responsePacket.responseCode = (RemoteFlashResponseCode)Functions.CharToInt(header);
            Parentform.serialPort1.Read(header,0,4);
            responsePacket.dataLength = Functions.CharToInt(header);
            Parentform.serialPort1.Read(header,0,responsePacket.dataLength);
            responsePacket.data = header;
            Parentform.serialPort1.Read(header,0,4);
            responsePacket.crc32 = Functions.CharToInt(header);
    //serial->receiveString((char*)&responsePacket.responseCode, sizeof(RemoteFlashResponseCode), serialTimoutRetries);
    //serial->receiveString((char*)&responsePacket.dataLength, sizeof(int), serialTimoutRetries);
    //serial->receiveString((char*)responsePacket.data, responsePacket.dataLength, serialTimoutRetries);
    //serial->receiveString((char*)&responsePacket.crc32, sizeof(unsigned int), serialTimoutRetries);
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
                return false;
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
       //     if (!serial->openPort(previousBaudrate))
	//	return false;

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
	        //if (!serial->sendString((const char*)enterProgrammingModeCommand, sizeof(enterProgrammingModeCommand)))
	        //	return false;
            //Thread.Sleep(2048*10/programmingBaudrate+1);
	        //sleep(2048*10/programmingBaudrate+1);	//2048=Maximum buffer size of UART in FPGA


            ///////Uncomment these
           // Parentform.serialPort1.Close();
           // Parentform.serialPort1.BaudRate= programmingBaudrate;
           // Parentform.serialPort1.Open();
	        //serial->closePort();

	        //if (!serial->openPort(programmingBaudrate))
	        //	return false;

	        // Flush serial buffer
	        //serial->clearReceiveBuffer();

	        commandPacket.setHelloCommand();
	        char[] header =
	        {
			        Functions.REMOTE_FLASH_PACKET_HEADER0, Functions.REMOTE_FLASH_PACKET_HEADER1,
			        Functions.REMOTE_FLASH_PACKET_HEADER2, Functions.REMOTE_FLASH_PACKET_HEADER3
	        };
            var tempint = Functions.ChartoByte(header);
            Parentform.Serial1_Write(tempint,0,4);
            commandPacket.ToCharArray();
            tempint = Functions.ChartoByte(commandPacket.objectdescriber);
            Parentform.Serial1_Write(tempint,0,commandPacket.objectdescriber.Length);
            commandPacket.insertChecksum();
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
            char[] line = new char[1];
            while (true)
            {
                line = Parentform.serialPort1.ReadExisting().ToCharArray();
                if (line.Length > 0)
                    break;
            }
            if (
                 line[0] != Functions.REMOTE_FLASH_PACKET_HEADER0 ||
                 line[1] != Functions.REMOTE_FLASH_PACKET_HEADER1 ||
                 line[2] != Functions.REMOTE_FLASH_PACKET_HEADER2 ||
                 line[3] != Functions.REMOTE_FLASH_PACKET_HEADER3
             )
		        return false;

	        responsePacket.reset();
            Array.Copy(line, 4, header, 0, 4);
            responsePacket.responseCode = (RemoteFlashResponseCode)Functions.CharToInt(header);
            Array.Copy(line, 8, header, 0, 4);
            responsePacket.dataLength = Functions.CharToInt(header);
            header = new char[responsePacket.dataLength];
            Array.Copy(line, 12, header, 0, responsePacket.dataLength);
            responsePacket.data = header;
            header = new char[4];
            Array.Copy(line, 12 + responsePacket.dataLength, header, 0, 4);
            responsePacket.crc32 = Functions.CharToInt(header);

	        if (!responsePacket.verifyChecksum())
		        return false;

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
                Parentform.serialPort1.Close();
            }
            connectionStatus = RemoteFlashConnectionStatus.REMOTE_FLASH_CONNECTION_STATUS_DISCONNECTED;
            return returnValue;
        }

        public char[] readbackSector(int sectorIndex)
        {
            char[] data = new char[Functions.FLASH_SECTOR_SIZE];
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
	        bool printProgress = true;
	        int i;
            var result = programSector(10, localMemory.sector[10].data);
	        for (i=0; i<Functions.FLASH_MEMORY_nSECTORS; i++)
	        {
		        remoteChecksum=0;
		        if (printProgress)
		        {
			        if ((i%64)==0)
			        {
                        Parentform.SetProgress(i * 100 / Functions.FLASH_MEMORY_nSECTORS);
				        //printf("\b%d%%", (i*100/FLASH_MEMORY_nSECTORS));
				        //fflush(stdout);
			        }
			        else
			        {

			        }
		        }
		        else
			        printProgress = true;

                try
                {
                    remoteChecksum = getSectorChecksum(i);
                }
                catch
                {
                    //throw new Exception("getting checksum failed. retrying");
                    i--;
                    continue;
                }

                var sectorchecksum = localMemory.sector[i].checksum();
		        if (remoteChecksum == sectorchecksum)
			        continue;

		        if (!programSector(i, localMemory.sector[i].data))
		        {
			        //retry to program
			        //fprintf(stderr, "Retrying to program sector %d.\r\n", i);
                    //throw new Exception
			        continue;
		        }
		        // Programming sector successful, make sure you verify it again
		        i--;
		        printProgress = false;
	        }
            Parentform.SetProgress(0);
	        //printf("\b100%\r\n");
	        remoteChecksum=getChipChecksum();
	        if (remoteChecksum < 0 )
	        {
		        //fprintf(stderr, "Failed to read chip checksum.\r\n");
		        return false;
	        }
	        if (remoteChecksum != localMemory.checksum())
	        {
		        //fprintf(stderr, "Chip checksum mismatch.\r\n");
		        return false;
	        }
	        //printf("Programming complete.\r\n");
	        return true;
        }
        public bool verifyChecksum(FlashMemory localMemory)
        {
	        int remoteChecksum;
	        bool printProgress = true;
	        for (int i=0; i<Functions.FLASH_MEMORY_nSECTORS; i++)
	        {
		        remoteChecksum=0;
		        if (printProgress)
		        {
			        if ((i%64)==0)
			        {
				        //printf("\b%d%%", (i*100/FLASH_MEMORY_nSECTORS));
				        //fflush(stdout);
			        }
			        else
			        {
				        switch (i%8)
				        {
				        case 0:
					        //printf("\b.");
					        //fflush(stdout);
					        break;
				        case 1:
					        //printf("-");
					        //fflush(stdout);
					        break;
				        case 2:
					        //printf("\b\\");
					        //fflush(stdout);
					        break;
				        case 3:
					        //printf("\b|");
					        //fflush(stdout);
					        break;
				        case 4:
					        //printf("\b/");
					        //fflush(stdout);
					        break;
				        case 5:
					        //printf("\b-");
					        //fflush(stdout);
					        break;
				        case 6:
					        //printf("\b\\");
					        //fflush(stdout);
					        break;
				        case 7:
					        //printf("\b|");
					        //fflush(stdout);
					        break;
				        default:
					        break;
				        }
			        }
		        }
		        else
			        printProgress = true;

                try
                {
                    remoteChecksum = getSectorChecksum(i);
                    //if (remoteChecksum == -1)
                    //{
                    //    i--;
                    //    continue;
                    //}
                }
                catch
                {
                    throw new Exception("getting checksum failed. retrying");
                    i--;
                    continue;
                }

                var temp = localMemory.sector[i].checksum();
		        if (remoteChecksum != localMemory.sector[i].checksum())
		        {
			        //printf("\r\nFlash verification failed, mismatch in sector %d.\r\n", i);
                    //throw new Exception("Flash verification failed, mismatch in sector " + i.ToString());
			        return false;
		        }
	        }
	        remoteChecksum=getChipChecksum();
	        if (remoteChecksum < 0)
	        {
		        //fprintf(stderr, "Failed to read chip checksum.\r\n");
		        return false;
	        }
	        if (remoteChecksum != localMemory.checksum())
	        {
		        //printf("Chip checksum mismatch.\r\n");
		        return false;
	        }
	        //printf("Checksum verification successful.\r\n");
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
        public char[] data;
        public int crc32;

        public RemoteFlashResponsePacket()
        {
            data = new char[Functions.FLASH_SECTOR_SIZE];
            reset();
        }
        public void reset()
        {
            responseCode = RemoteFlashResponseCode.REMOTE_FLASH_RESPONSE_CODE_INVALID;
            dataLength = 0;
            for (int i = 0; i < Functions.FLASH_SECTOR_SIZE; i++)
                data[i] = (char)0xFF;
            crc32 = 0;
        }
        public bool verifyChecksum()
        {
            char[] tochecksum = new char[8 + dataLength];
            int responsecodeint = (int)responseCode;
            for (int i = 0; i < 4; i++)
            {
                tochecksum[i] = (char)(responsecodeint % 256);
                responsecodeint /= 256;
            }
            responsecodeint = dataLength;
            for (int i = 0; i < 4; i++)
            {
                tochecksum[i+4] = (char)(responsecodeint % 256);
                responsecodeint /= 256;
            }
            for (int i = 0; i < dataLength; i++)
                tochecksum[i + 8] = data[i];
            int localChecksum = Functions.crc32b(tochecksum, 8+dataLength,0);
	        if (localChecksum == crc32)
		        return true;

	    //printf("Warning: Checksum failed!\r\n");
            throw new Exception("Checksum Error");
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
