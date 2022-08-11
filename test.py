import cv2 as cv
import numpy as np
import time
start_time = time.time()
img3=None
capture=cv.VideoCapture('piano.mp4')
count=0
width=0
height=0
while True:
    _,img= capture.read()
    if img is None:
        break
    gray=cv.cvtColor(img,cv.COLOR_BGR2GRAY)
    mask = cv.inRange(gray, 95, 250)
    output = cv.bitwise_and(img, img, mask = mask)
    slika=output[int(mask.shape[0]//2.9):int(mask.shape[0]//2.9)+1,0:int(mask.shape[1])][0]
    if count==0:
        img3=slika
        width=mask.shape[1]
        height=mask.shape[0]
    else:
        img3=np.concatenate((slika,img3))
    count+=1
img3=img3.reshape(count,width,3)
capture.release()
cv.imwrite('prototip.png',cv.resize(img3,(width,int(count*2))))
print("--- %s seconds ---" % (time.time() - start_time))







