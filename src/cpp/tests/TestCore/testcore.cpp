#include <toecore.h>
#include <stdio.h>

#define ASSERT(flag, msg) { if (!(flag)) printf("Test fail: %s", msg); return 1; }

int Test32Bit()
{
	//ASSERT(sizeof(void*)==8,"Not fixed to 32 bit");
	ASSERT(sizeof(ToeHashString(""))==4,"Not fixed to 32 bit");
	return 0;
}

int main()
{
	int errcode = 0;
	errcode |= Test32Bit();
	if (errcode != 0)
	{
		printf("One or more tests failed!");
	}
	return errcode;
}