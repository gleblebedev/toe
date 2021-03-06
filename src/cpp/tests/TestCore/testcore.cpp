#include <toecore.h>
#include <toemsgreg.h>
#include <stdio.h>

#define ASSERT(flag, msg) { if (!(flag)) printf("Test fail " ": %s", msg); return 1; }

int Test32Bit()
{
	//ASSERT(sizeof(void*)==8,"Not fixed to 32 bit");
	ASSERT(sizeof(ToeHashString(""))==4,"Not fixed to 32 bit");
	return 0;
}
ToeMessageField testMsg1Fields[] = {{"Field1",ToeHashString("int"),1},{"Field2",ToeHashString("int"),1}};

ToeMessage testMsg1 = {0,"Msg",2,testMsg1Fields};
ToeMessage testMsg1a = {0,"Msg",0,testMsg1Fields};
ToeMessage testMsg2 = {0,"Msg2",0,0};
ToeMessage testMsg2a = {0,"Msg2",0,0};
ToeMessage testMsg3 = {0,"Msg3",0,0};
ToeMessage testMsg3a = {0,"Msg3",0,0};

int TestReg123(ToeMessage*a,ToeMessage*aa,ToeMessage*b,ToeMessage*bb,ToeMessage*c,ToeMessage*cc)
{
	ToeMessageRegistry* reg;
	int res = 0;
	reg = ToeCreateMessageRegistry(100);

	if (ToeRegisterMessage(reg,a)!= TOE_REG_SUCESS)
	{
		++res;
		puts("Can't register first message");
	}
	if (ToeRegisterMessage(reg,aa)!= TOE_REG_DIFFERENT_MESSAGE_ALREADY_REGISTERED)
	{
		++res;
		puts("Didn't find the same message message");
	}
	if (ToeRegisterMessage(reg,b)!= TOE_REG_SUCESS)
	{
		++res;
		puts("Can't register first message");
	}
	if (ToeRegisterMessage(reg,bb)!= TOE_REG_DIFFERENT_MESSAGE_ALREADY_REGISTERED)
	{
		++res;
		puts("Didn't find the same message message");
	}
	if (ToeRegisterMessage(reg,c)!= TOE_REG_SUCESS)
	{
		++res;
		puts("Can't register first message");
	}
	if (ToeRegisterMessage(reg,cc)!= TOE_REG_DIFFERENT_MESSAGE_ALREADY_REGISTERED)
	{
		++res;
		puts("Didn't find the same message message");
	}
	ToeDestroyMessageRegistry(reg);
	return res;
}

int TestRoutingNull()
{
	if (ToePrapareMessageRoutingTable(0,0) != TOE_SUCCESS)
		return 1;
	return 0;
}
int TestRoutingNull2()
{
	if (ToePrapareMessageRoutingTable(0,0) == TOE_SUCCESS)
		return 1;
	return 0;
}
int TestRouting0()
{
	ToeMessageRoute routes[1];
	if (ToePrapareMessageRoutingTable(routes,0) != TOE_SUCCESS)
		return 1;
	return 0;
}

int TestRouting1()
{
	ToeMessageRoute routes[1];
	if (ToePrapareMessageRoutingTable(routes,1) != TOE_SUCCESS)
		return 1;
	return 0;
}

int TestRouting2()
{
	ToeMessageRoute routes[2];
	if (ToePrapareMessageRoutingTable(routes,2) != TOE_SUCCESS)
		return 1;
	return 0;
}

int TestRouting3()
{
	ToeMessageRoute routes[3];
	if (ToePrapareMessageRoutingTable(routes,3) != TOE_SUCCESS)
		return 1;
	return 0;
}

int main()
{
	int errcode = 0;
	//errcode |= Test32Bit();
	errcode |= TestReg123(&testMsg1,&testMsg1a, &testMsg2,&testMsg2a, &testMsg3,&testMsg3a);
	errcode |= TestReg123(&testMsg1,&testMsg1a, &testMsg3,&testMsg3a, &testMsg2,&testMsg2a);
	errcode |= TestReg123(&testMsg2,&testMsg2a, &testMsg1,&testMsg1a, &testMsg3,&testMsg3a);
	errcode |= TestReg123(&testMsg2,&testMsg2a, &testMsg3,&testMsg3a, &testMsg1,&testMsg1a);
	errcode |= TestReg123(&testMsg3,&testMsg3a, &testMsg1,&testMsg1a, &testMsg2,&testMsg2a);
	errcode |= TestReg123(&testMsg3,&testMsg3a, &testMsg2,&testMsg2a, &testMsg1,&testMsg1a);

	errcode |= TestRoutingNull();
	errcode |= TestRoutingNull2();
	errcode |= TestRouting0();
	errcode |= TestRouting1();
	errcode |= TestRouting2();
	errcode |= TestRouting3();

	if (errcode != 0)
	{
		printf("One or more tests failed!");
		char b[128];
		gets(b);
	}
	return errcode;
}