#Libraries
import serial
import numpy as np
import pandas as pd
from matplotlib import pyplot as plt
import threading
import array
import time
from IPython.display import display

def main_loop():
    global flag
    global past
    global key
    
    flag = True
    ch = input("To stop please input 'e' key: ")
    print(ch)
    if ch == "e":
        print("e key triggered")
        
        flag = False
    else:
        print("Invalid input.")
        main_loop()        
  
def measurement_loop():
    #Setting measurement format request
    checksum= 0x44+0x0a+0x01+0x00
    a_data=array.array('B',[0x44,0x0a,0x01,checksum,0x00])
    ser.write(a_data)
    ans=ser.read(5) 
    
    #Register batch command
    checksum= 0x44+0x02+0x03+0x00+0x00+0x01
    a_data=array.array('B',[0x44,0x02,0x03,checksum,0x00,0x00,0x01])
    ser.write(a_data)
    ans=ser.read(5)
    
    print('measurement started')
    
    y0= np.array([])
    y1= np.array([])
   
    t= np.array([])
    
    past = time.time()
    while flag:
        now = time.time()

        # Ask for data
        checksum = (0x04 + 0x03 + 0x00)
        b_data = array.array('B', [0x04, 0x03, 0x00, checksum])
        ser.write(b_data)

        sampled = ser.read(9)  # Read full data packet
        if len(sampled) < 9:
            print("Incomplete packet")
            continue

        # Process channels
        tmp0 = sampled[5] + sampled[6]*256   # channel 0
        tmp1 = sampled[7] + sampled[8]*256   # channel 1

        y0 = np.append(y0, tmp0)
        y1 = np.append(y1, tmp1)
        t = np.append(t, now - past)

        # Optional: live plot
        # Keep only last 10 seconds of data
        window = 10  # seconds
        mask = (t >= (now - past - window))
        
        plt.clf()
        plt.plot(t[mask], y0[mask], label='ts0')
        plt.plot(t[mask], y1[mask], label='ts1')
        plt.xlabel('Time [sec]')
        plt.ylabel('Count Value')
        plt.grid()
        plt.legend()
        plt.pause(0.001)
        
   
    ser.close()
        
        
    fig, ax4 = plt.subplots()
    ax4.scatter(t, y0, s=5,label="ts0")
    ax4.scatter(t, y1, s=5,label="ts1")
        
    plt.xlabel('Time [sec]')
    plt.ylabel('Count Value')
    plt.legend(ncol=2,loc="lower left")
    plt.grid()
        
#Serial Communication Settings
ser = serial.Serial()
ser.baudrate = 38400
ser.port = "COM4"
ser.open()

t1 = threading.Thread(target=main_loop)
t1.start()
measurement_loop()