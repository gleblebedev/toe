#include <toecore.h>
#include <toemsgreg.h>
#include <stdlib.h>

struct ToeMessageRegistry
{
	unsigned long registrySize;
	unsigned long numMessages;
	ToeMessage** messages;
};

ToeMessageRegistry* ToeCreateMessageRegistry(unsigned long maxNumMessages)
{
	ToeMessageRegistry* r = (ToeMessageRegistry*)malloc(sizeof(ToeMessageRegistry));
	r->registrySize = maxNumMessages;
	r->numMessages = 0;
	r->messages = (ToeMessage**)malloc(sizeof(ToeMessage*)*maxNumMessages);
	return r;
}
int ToeQuickSearch(ToeMessageRegistry* reg, unsigned long messageId, unsigned int* l)
{
	unsigned long left = 0;
	/* This internal func is never called if reg->numMessages == 0*/
	unsigned long right = reg->numMessages-1;
	while ((long)left <= (long)right) {
		// we don't need fancy calculation since sum will never overflow
		unsigned long middle = (left+right)>>1;
		register unsigned long id = reg->messages[middle]->Id;
		if (id == messageId)
		{
			*l = middle;
			return 1;
		}
		if (id > messageId)
		{
			right = middle-1;
		}
		else
		{
			left = middle+1;
		}
	}
	*l = left;
	return 0;
}
ToeMessage* ToeFindMessage(ToeMessageRegistry*reg, unsigned long messageId)
{
	unsigned int pos;
	if (ToeQuickSearch(reg,messageId,&pos))
	{
		return reg->messages[pos];
	}
	return 0;
}

TOE_REG_RESULT ToeRegisterMessage(ToeMessageRegistry* reg, ToeMessage* message)
{
	unsigned int pos = 0;
	unsigned int i;
	unsigned int size;

	message->Id = ToeHashString(message->Name);

	if (reg->numMessages > 0 && ToeQuickSearch(reg,message->Id,&pos))
	{
		if (reg->messages[pos] == message)
			return TOE_REG_SUCESS;

		//TODO: check if the message is same
		return TOE_REG_DIFFERENT_MESSAGE_ALREADY_REGISTERED;
	}

	if (message->ParentId)
	{
		if (!ToeQuickSearch(reg,message->ParentId,&i))
		{
			return TOE_REG_PARENT_NOT_FOUND;
		}
		message->Parent = reg->messages[i];
	}
	else
	{
		message->Parent = 0;
	}

	for (i=reg->numMessages; i>pos; --i)
	{
		reg->messages[i] = reg->messages[i-1];
	}
	reg->messages[pos] = message;
	++reg->numMessages;

	size = (message->Parent)?message->Parent->InitialSize:0;
	for (i=0; i<message->NumFields; ++i)
	{
		ToeMessageField* field = message->Fields+i;
		field->Offset = size;
	}
	message->InitialSize = size;

	return TOE_REG_SUCESS;
}

void ToeDestroyMessageRegistry(ToeMessageRegistry* reg)
{
	free(reg->messages);
	free(reg);
}