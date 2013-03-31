del /Q Emscripten\toe_Debug\*
mkdir Emscripten
mkdir Emscripten\toe_Debug

C:\MyWork\emscripten\emcc.exe -o Emscripten/toe_Debug/main.o -DEMSCRIPTEN -D__STDC__ -MD -MF Emscripten/toe_Debug/main.d -O0 -x c++ src/main.cpp
C:\MyWork\emscripten\emcc.exe Emscripten/toe_Debug/main.o Emscripten/toe_Debug/main.o -o Emscripten/toe_Debug/toe.html
"Emscripten/toe_Debug/toe.html"