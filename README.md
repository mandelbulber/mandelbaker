# mandelbaker - bake your custom mandelbrot

## mandelbaker? What's that? 

The mandelbaker program allows you to render beautiful mandelbrot fractals in the form of images.
You may adjust the resolution and the amount of maximum iterations of the mandelbrot, which can be done in the header of the mandelbaker.py file.

## Current features & future features

The first big feature of this calculator is the inbuilt multiprocessing handler. At the moment, the final image is split in four parts, which are being rendered by four different processes. This drastically improves the performance because four instead of just one CPU kernal can be used.

There are plans to improve this in future, so the user is able to set the amount of used processes (used CPU kernals).

Im open to recieve any suggestions for further features and improvements, so please let me know if you have any ideas :)

## Usage & marketing

The mandelbaker is completely open-source. Everyone is allowed to use and adjust it to achieve the best personal experience - for free!

## Install-Guide

The mandelbaker uses some libraries, which may not be install on your personal device yet. To do so, you can simply use the command 'pip install -r requirements.txt'. This will automatically install all required packages.