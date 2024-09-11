# Serial Communication 

## Language
1. C#
2. GUI - Winform

## Function
1. Check Connection Status
2. Visualize Status with two-state Button (Currently COM6 Only)
3. 6 Ports Supported (Open Close)

## Control
1. Button ( Changes ORANGE <-> Gray )


## For Developers

If you're using ONE port, then it might be :  

```SerialPort serialport``` 

In circumstances where you need MULTIPLE ports :  

```csharp
SerialPort serialport1;  
SerialPort serialport2;  
SerialPort serialport3;  
SerialPort serialport4;  
SerialPort serialport5;  
```

Upper code has multiple Instances of Serialport. 
Its beginner c++ style, need to be modified.  

```csharp
List<SerialPort> _serialport = new List<SerialPort>;  
var portNames = new List<string> { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6" };  
var baudRate = 115200;              
foreach (var portName in portNames)
{
    var serialPort = new SerialPort(portName, baudRate);
    _serialport.Add(serialPort);
}
```
Created a List that contains SerialPort Instances  
This reduces 6 lines of declaration to 1 line of code  
Foreach is like a python. Its a good tool.  
Furthermore, when it comes to checking connection status of each 6 Active Ports,  
There are at many ways to achieve  

```csharp
CheckConnection()
{
    DateTime CurrentTime = DateTime.Now;
foreach (var SQLiteData in SQLiteDatas)
    {
        string COM_Number = SQLiteData.Key;
        DateTime TimeInformation_DB = SQLiteData.Value;
        if ((CurrentTime - TimeInformation_DB).TotalSeconds > timeout)
        {
            //GUI Update - Connection Lost
        }  

Sequence Flow
Get SQLiteDB's last data saved time Information  

Get Current Time
Compare Two  
if Current Time is larger than SqLite, think of it as connection lost.
set Timeout 5 seconds
```

Consider Two as Same  
Connected = Data Receiving every second  
Used in Program which saves Serial Communication Data in SQLiteDB  


```
Add Extra Method
Method (parameter : COM port number)
returns : comport_number.IsOpen property
```
not bad, though method is called by Timer every second to check connection status.  

```
Using Dictionary
        private Dictionary<string, SerialPort> _serialPort;
 
```
There is no need to bind Connection status and Serial Port Instance together.  
Because there is already IsOpen boolean property in SerialPort Instance  


