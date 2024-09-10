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

'''SerialPort serialport''' 

In circumstances where you need MULTIPLE ports :  

'''
SerialPort serialport1  
SerialPort serialport2  
SerialPort serialport3  
SerialPort serialport4  
SerialPort serialport5  
...  
'''

Upper code has multiple Instances of Serialport. 
Its beginner c++ style, need to be modified.  

'''
List<SerialPort> _serialport = new List<SerialPort>;  
var portNames = new List<string> { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6" };  
var baudRate = 115200;              
foreach (var portName in portNames)
{
    var serialPort = new SerialPort(portName, baudRate);
    _serialport.Add(serialPort);
}
'''

