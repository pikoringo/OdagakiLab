#ライブラリのインポート
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
    ch = input("停止する場合は eキーを入力してください：")
    print(ch)
    if ch == "e":
        print("eが入力されました")
        
        flag = False
    else:
        print("e以外が入力されました")
        main_loop()        
  
def measurement_loop():
    #計測方式番号の設定を要求する
    checksum= 0x44+0x0a+0x01+0x00
    a_data=array.array('B',[0x44,0x0a,0x01,checksum,0x00])
    ser.write(a_data)
    ans=ser.read(5) 
    
    #バッチコマンドの登録
    checksum= 0x44+0x02+0x03+0x00+0x00+0x01
    a_data=array.array('B',[0x44,0x02,0x03,checksum,0x00,0x00,0x01])
    ser.write(a_data)
    ans=ser.read(5)
    
    print('measurement started')
    
    y0= np.array([])
   
    t= np.array([])
    
    past = time.time()
    while flag:
        now = time.time()
        #カウント値の要求
        checksum = 0x04 + 0x03 + 0x00 
        b_data = array.array('B',[0x04,0x03,0,checksum])
        ser.write(b_data)
        sampled = ser.read(7)
        print("Raw sampled:", sampled.hex(), "->", list(sampled))

        #while(i<5):
        tmp = sampled[6]*2**8+sampled[5]
            
        #if(i==5):
        y0=np.append(y0,tmp)
        t=np.append(t,now-past)
        np.savetxt('Sakaki-after.txt',np.vstack([t, y0]).T,delimiter=',' ,header='time,ts0')
                #past=now
       
            #i=i+2
        xlen=t[-1]-5
        ylen=t[-1]+5
        plt.xlim(xlen,ylen)
        line, = plt.plot(t,y0,'-',color='b')
        
        plt.pause(0.001)
        line.remove()
        
    else:
        ser.close()
        #データをcsv出力
        
        #np.savetxt('data.txt',np.vstack([t, y0, y1,y3,y5,y7,y9,y11,y13,y15,y17,y19,y21,y23,y27,y29,y30,y31,y32,y34,y35]).T,delimiter=',' ,header='time,ts0,ts1,ts3,ts5,ts7,ts9,ts11,ts13,ts15,ts17,ts19,ts21,ts23,ts27,ts29,ts30,ts31,ts32,ts34,ts35')
        #np.savetxt('data.txt',np.vstack([t, y0, y1,y3,y5,y7,y9,y11,y13,y15,y17,y19,y21,y23,y27,y29,y30,y31,y32,y34,y35]).T,delimiter=',' ,header='time,ts0,ts1,ts3,ts5,ts7,ts9,ts11,ts13,ts15,ts17,ts19,ts21,ts23,ts27,ts29,ts30,ts31,ts32,ts34,ts35',comments="%")
        np.savetxt('Sakaki-after.txt',np.vstack([t, y0]).T,delimiter=',' ,header='time,ts0')
        #np.savetxt('data.txt',np.vstack([t, y0, y1,y3,y5,y7,y9,y11,y13,y15,y17]).T,delimiter=',' ,header='time,ts0,ts1,ts3,ts5,ts7,ts9,ts11,ts13,ts15,ts17',comments="%")
        #read_text_file = pd.read_csv (r"C:\修士研究\シリアル通信\data.txt")                                                                                              
        #read_text_file.to_csv (r"C:\修士研究\シリアル通信\new_data1.csv", index=None)
        #read_text_file = pd.read_csv (r"C:data.txt")                                                                                              
        #read_text_file.to_csv (r"C:new_data1.csv", index=None)
        #データのグラフ化
        
        fig, ax4 = plt.subplots()
        ax4.scatter(t, y0, s=5,label="ts0")
        
        plt.xlabel('Time [sec]')
        plt.ylabel('Count Value')
        plt.legend(ncol=2,loc="lower left")
        plt.grid()
        
#シリアル通信の設定
ser = serial.Serial()
ser.baudrate = 38400
ser.port = "COM4"
ser.open()

t1 = threading.Thread(target=main_loop)
t1.start()
measurement_loop()