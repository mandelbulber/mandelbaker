#!/usr/bin/env python3
# -*- coding: utf-8 -*-
# |---------------------------------------.*-General-Information-*.----------------------------------------|
# Created By  : Eugster Andrin
# Created Date: 09/06/2022
# version ='1.0'
# |---------------------------------------.*-Program-Description-*.----------------------------------------|
""" This module represents a calculator used to render manderbrot fractals.                              """
""" To achieve the perfect mandelbrot of your personal liking, feel free to adjust the variables below.  """
""" Be aware that this program is still WIP, so please don't set your expectations too high.             """
""" There are still some (especially performance) improvements to be done, but I hope you can already    """
""" enjoy the current state of this project.                                                             """
""" For further information, have a look at the README.md.                                               """
# |--------------------------------------------.*-Libraries-*.---------------------------------------------|
import os
import datetime
from PIL import Image, ImageDraw
from multiprocessing import Process, Queue
# |-----------------------------------------.*-User-Adjustments-*.-----------------------------------------|
height_of_mandelbrot = 512 #   512 | 1080 | 3840 | 7680 | 15360 | 30720 | 61440 | 122880 | 245760 | 491520 |
width_of_mandelbrot  = 512 #              |  4K  |  8K  |  16K  |  32K  |  64K  |  128K  |  256K  |  512K  |
max_iterations = 255
number_of_processes = 1 # not integrated yet
results_folder = "C:\mandelbaker\\results"
# |--------------------------------------------------------------------------------------------------------|

def mandelbrot_calc():
    imageQuartalsList = [(0, 0), (width_of_mandelbrot//2, 0), (0, height_of_mandelbrot//2), (width_of_mandelbrot//2, height_of_mandelbrot//2)]
    queues = []
    processes = []
    result = []

    for x, y in imageQuartalsList:
        q = Queue()
        queues.append(q)
        p = Process(target=partialcalculator, args=(q, x, y))
        p.start()
        processes.append(p)
    
    for q in queues:
        tempList = []
        tempList.append(q.get())
        result.append(tempList)

    for p in processes:
        p.join()
        
    return result, imageQuartalsList

def partialcalculator(queue, startX, startY):
    x1 = -2.0
    x2 = 1.0
    y1 = -1.5
    y2 = 1.5
    myList = []
    for y in range(startY, startY + height_of_mandelbrot//2):
        zy = y * (y2- y1) / (height_of_mandelbrot - 1)  + y1
        for x in range(startX, startX + width_of_mandelbrot//2):
            zx = x * (x2 - x1) / (width_of_mandelbrot - 1)  + x1
            z = zx + zy * 1j
            c = z
            for i in range(max_iterations):
                if abs(z) > 2.0: break
                z = z * z + c
            myList.append(i)
    queue.put(myList)

def draw(calcParam):
    print("start drawing at: " + str(datetime.datetime.now()))
    lists, quartals = calcParam
    image = Image.new("RGB", (width_of_mandelbrot, height_of_mandelbrot))
    
    for listindex, list in enumerate(lists):
        xytemp = quartals[listindex]
        xtemp = xytemp[0]
        ytemp = xytemp[1]
        for sublist in list:
            for index, i in enumerate(sublist):
                x = 0
                y = 0
                x = xtemp + index % (width_of_mandelbrot//2)
                y = ytemp + index // (width_of_mandelbrot//2)
                image.putpixel((x, y), (i % 3 * 64, i % 8 * 32, i % 16 * 16))
    return image

def program():
    start_time = datetime.datetime.now()
    image = draw(mandelbrot_calc())
    end_time = datetime.datetime.now()
    duration_of_calculation = end_time - start_time
    timestamp = ImageDraw.Draw(image)
    timestamp.text((0, 0), str(duration_of_calculation))
    if not os.path.exists(results_folder):
        os.makedirs(results_folder)
    image.save(results_folder + "\mandelbrot_" + str(width_of_mandelbrot) + "x" + str(height_of_mandelbrot) + ".png", "PNG")
    image.show()

if __name__ == "__main__":
    program()