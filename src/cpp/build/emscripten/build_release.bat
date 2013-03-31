del /Q Emscripten\toe_Release\*
mkdir Emscripten
mkdir Emscripten\toe_Release

C:\MyWork\emscripten\emcc.exe -o Emscripten/toe_Release/main.o -DEMSCRIPTEN -D__STDC__ -MD -MF Emscripten/toe_Release/main.d -O2 -x c++ src/main.cpp
C:\MyWork\emscripten\emcc.exe Emscripten/toe_Release/main.o Emscripten/toe_Release/main.o -o Emscripten/toe_Release/toe.html
REM -O3 --closure 1 --minify 1

"Emscripten/toe_Release/toe.html"