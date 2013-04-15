#include <toecore.h>
#include <toemsgreg.h>
#include <stdio.h>
#include <stdlib.h>

extern "C"{
#include <lua.h>
#include <lualib.h>
#include <lauxlib.h>
}

#define ASSERT(flag, msg) { if (!(flag)) printf("Test fail: %s", msg); if (L) lua_close(L); return 1; }

void bail(lua_State *L, char *msg){
	fprintf(stderr, "\nFATAL ERROR:\n  %s: %s\n\n",
		msg, lua_tostring(L, -1));
	exit(1);
}

int pmain(lua_State *L)
{
	return 0;
}

int Test1()
{
	lua_State* L;
	L=luaL_newstate();
	//ASSERT(L,"cannot create state: not enough memory")

	lua_pushcclosure(L,&pmain,0);
	lua_setglobal(L,"pmain");

	luaL_openlibs(L);

	if (luaL_loadfile(L, "test1.lua")) bail(L, "luaL_loadfile() failed");
	lua_call(L,0,0);



	//lua_pushinteger(L,argc);
	//lua_pushlightuserdata(L,argv);
	//if (lua_pcall(L,2,0,0)!=LUA_OK) fatal(lua_tostring(L,-1));
	lua_close(L);
	return 0;
}

int main()
{
	

	int errcode = 0;
	errcode |= Test1();
	if (errcode != 0)
	{
		printf("One or more tests failed!");
	}
	return errcode;
}