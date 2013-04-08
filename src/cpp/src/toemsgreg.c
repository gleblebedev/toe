#include <toemsgreg.h>

struct ToeMessageRegistry
{
	
};

typedef struct ToeMessageRegistry* ToeMessageRegistry;

ToeMessageRegistry* ToeCreateMessageRegistry(unsigned long maxNumMessages,unsigned long maxNumFields);

void ToeDestroyMessageRegistry(ToeMessageRegistry*);