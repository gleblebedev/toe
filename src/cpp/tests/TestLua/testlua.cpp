#include <toecore.h>
#include <stdio.h>
extern "C"{
#include <lua.h>
#include <lauxlib.h>
}

#define ASSERT(flag, msg) { if (!(flag)) printf("Test fail: %s", msg); if (L) lua_close(L); return 1; }

int Test1()
{
	lua_State* L;
	L=luaL_newstate();
	ASSERT(L,"cannot create state: not enough memory")
	//lua_pushcfunction(L,&pmain);
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