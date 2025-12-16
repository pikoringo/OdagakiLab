#ライブラリのインポート
import serial
import numpy as np
import pandas as pd
from matplotlib import pyplot as plt
import threading
import array
import time
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
    y1= np.array([])
    y3= np.array([])
    y5= np.array([])
    y7= np.array([])
    y9= np.array([])
    y11= np.array([])
    y13= np.array([])
    y15= np.array([])
    y17= np.array([])
    
    #y19= np.array([])
    #y21= np.array([])
    #y23= np.array([])
    #y27= np.array([])
    #y29= np.array([])
    #y30= np.array([])
    #y31= np.array([])
    #y32= np.array([])
    #y34= np.array([])
    #y35= np.array([])
    t= np.array([])

    past = time.time()
    
    while flag:
        now = time.time()
        
        #カウント値の要求

        checksum = 0x04 + 0x03 + 0x00 
        b_data = array.array('B',[0x04,0x03,0,checksum])
        ser.write(b_data)
        sampled = ser.read(45)
        i=5
        while(i<44):
            tmp = sampled[i+1]*2**8+sampled[i]
            
            if(i==5):
                y0=np.append(y0,tmp)
                t=np.append(t,now-past)
                #past=now
            elif(i==7):     
                y1=np.append(y1,tmp)
            elif(i==9):     
                y3=np.append(y3,tmp)
            elif(i==11):     
                y5=np.append(y5,tmp)
            elif(i==13):     
                y7=np.append(y7,tmp)
            elif(i==15):     
                y9=np.append(y9,tmp)
            elif(i==17):     
                y11=np.append(y11,tmp)
            elif(i==19):     
                y13=np.append(y13,tmp)
            elif(i==21):     
                y15=np.append(y15,tmp)
            elif(i==23):     
                y17=np.append(y17,tmp)
                
            """elif(i==25):     
                y19=np.append(y19,tmp)
            elif(i==27):     
                y21=np.append(y21,tmp)
            elif(i==29):     
                y23=np.append(y23,tmp)
            elif(i==31):     
                y27=np.append(y27,tmp)
            elif(i==33):     
                y29=np.append(y29,tmp)
            elif(i==35):     
                y30=np.append(y30,tmp)
            elif(i==37):     
                y31=np.append(y31,tmp)
            elif(i==39):     
                y32=np.append(y32,tmp)
            elif(i==41):     
                y34=np.append(y34,tmp)
            else:     
                y35=np.append(y35,tmp)
            """    
            i=i+2

    else:
        ser.close()
        
        #データをcsv出力
        
        #np.savetxt('data.txt',np.vstack([t, y0, y1,y3,y5,y7,y9,y11,y13,y15,y17,y19,y21,y23,y27,y29,y30,y31,y32,y34,y35]).T,delimiter=',' ,header='time,ts0,ts1,ts3,ts5,ts7,ts9,ts11,ts13,ts15,ts17,ts19,ts21,ts23,ts27,ts29,ts30,ts31,ts32,ts34,ts35')
        #np.savetxt('data.txt',np.vstack([t, y0, y1,y3,y5,y7,y9,y11,y13,y15,y17,y19,y21,y23,y27,y29,y30,y31,y32,y34,y35]).T,delimiter=',' ,header='time,ts0,ts1,ts3,ts5,ts7,ts9,ts11,ts13,ts15,ts17,ts19,ts21,ts23,ts27,ts29,ts30,ts31,ts32,ts34,ts35',comments="%")
        np.savetxt('data.txt',np.vstack([t, y0, y1,y3,y5,y7,y9,y11,y13,y15,y17]).T,delimiter=',' ,header='time,ts0,ts1,ts3,ts5,ts7,ts9,ts11,ts13,ts15,ts17')
        np.savetxt('data.txt',np.vstack([t, y0, y1,y3,y5,y7,y9,y11,y13,y15,y17]).T,delimiter=',' ,header='time,ts0,ts1,ts3,ts5,ts7,ts9,ts11,ts13,ts15,ts17',comments="%")
        
        read_text_file = pd.read_csv (r"C:\修士研究\シリアル通信\data.txt")                                                                                              
        read_text_file.to_csv (r"C:\修士研究\シリアル通信\new_data1.csv", index=None)
        
        #データのグラフ化
        
        fig, ax4 = plt.subplots()
        ax4.scatter(t, y0, s=5,label="ts0")
        ax4.scatter(t, y1, s=5,label="ts1")
        
        ax4.scatter(t, y3, s=5,label="ts3")
        ax4.scatter(t, y5, s=5,label="ts5")
        ax4.scatter(t, y7, s=5,label="ts7")
        ax4.scatter(t, y9, s=5,label="ts9")
        ax4.scatter(t, y11, s=5,label="ts11")
        ax4.scatter(t, y13, s=5,label="ts13")
        ax4.scatter(t, y15, s=5,label="ts15")
        ax4.scatter(t, y17, s=5,label="ts17")
     
        #ax4.scatter(t, y19, s=5,label="ts19")
        #ax4.scatter(t, y21, s=5,label="ts21")
        #ax4.scatter(t, y23, s=5,label="ts23")
        #ax4.scatter(t, y27, s=5,label="ts27")
        #ax4.scatter(t, y29, s=5,label="ts29")
        #ax4.scatter(t, y30, s=5,label="ts30")
        #ax4.scatter(t, y31, s=5,label="ts31")
        #ax4.scatter(t, y32, s=5,label="ts32")
        #ax4.scatter(t, y34, s=5,label="ts34")
        #ax4.scatter(t, y35, s=5,label="ts35")
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