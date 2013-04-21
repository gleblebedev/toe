/*
 * Copyright (C) 2013 Gleb Lebedev.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this 
 * software and associated documentation files (the "Software"), to deal in the Software 
 * without restriction, including without limitation the rights to use, copy, modify, 
 * merge, publish, distribute, sublicense, and/or sell copies of the Software, and to 
 * permit persons to whom the Software is furnished to do so, subject to the following 
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies 
 * or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

#include <stdlib.h>
#include "toecoreint.h"
#include "toemsgreg.h"
#include "toecoremsgs.h"

#define TOE_ASSERT(a,b)
//void TOE_ASSERT(bool f, const char* errorMessage)
//{
//	if (!f)
//	{
//		exit(1);
//	}
//}

ToeMessageField toe_createSystemFields [] = {
	{
		/* Name = */	0,
		/* Type = */	0,
		/* Length = */	1
	}
};

ToeMessage toe_coreMessages[] = {
	{
		/* ParentId = */	0,
		/* Name = */		"CreateSystem",
		/* NumFields = */	0,
		/* Fields = */		toe_createSystemFields
	}
};

unsigned long ToeHashString(const char* string)
{
	unsigned long hash = 5381;
    int c;
    while ((c = *string++))
    {
        c = (c < 'A' || c > 'Z') ? c : (c - 'A' + 'a');
        hash = ((hash << 5) + hash) + c;
    }
    return hash;
}


TOE_RESULT ToeOnCreateSystemMessage(ToeScene* scene, void* context)
{

	return TOE_ERROR;
}

static ToeMessageRoute toeCoreCallbackTable[1] = {{"CreateSystem",ToeOnCreateSystemMessage}};

static int toe_coreCallbackTableInitialized = 0;

void ToeSetDefaultOptions(ToeSceneOptions* options)
{
	options->CreateSystemCallback = 0;
	options->CreateSystemCallbackContext = 0;
	options->MessageBufferSize = 128*1024;
}

ToeScene* ToeCreateScene(const ToeSceneOptions* options)
{
	ToeScene* scene;

	if (!toe_coreCallbackTableInitialized)
	{
		ToePrapareMessageRoutingTable(toeCoreCallbackTable,sizeof(toeCoreCallbackTable)/sizeof(ToeMessageRoute));
	}

	scene = (ToeScene*)malloc(sizeof(ToeScene));
	scene->totalLayers = 0;
	scene->totalSystems = 0;
	scene->options = *options;

	TOE_ASSERT(scene->options.MessageBufferSize > 256, "Message buffer should be at least 256 bytes");
	TOE_ASSERT(scene->options.MessageBufferSize <= 1073741824, "Message buffer should be less than 1GB");

	scene->messageBufferSize = 256;
	while (scene->messageBufferSize < scene->options.MessageBufferSize)
		scene->messageBufferSize = scene->messageBufferSize<<1;
	scene->messageBufferMask = scene->messageBufferSize-1;

	scene->messageBuffer = (unsigned char*)malloc(scene->messageBufferSize);

	scene->currentReadPosition = 0;
	scene->currentInMessageId = 0;

	scene->currentWritePosition = 0;
	scene->currentOutMessageId = 0;
	return scene;
}
void ToeDestroyScene(ToeScene* scene)
{
	free(scene->messageBuffer);
	free(scene);
}
TOE_MESSAGE_RESULT ToePorcessOtherMessage(ToeScene* scene, void* context)
{
	unsigned long layer,i;
	ToeMessageGetTargetLayer(scene, &layer);
	if (!layer) 
	{
		for (i=0; i<scene->totalLayers;++i) 
		{
			//if (TOE_MESSAGE_HANDELED == scene->layers[i].
		}
		return TOE_MESSAGE_IS_NOT_HANDELED;
	}
	for (i=0; i<scene->totalLayers;++i)
	{
		if (scene->layers[i].Id == layer)
		{
			//scene->layers[i].
		}
	}
	return TOE_MESSAGE_IS_NOT_HANDELED;
}
int ToePorcessMessage(ToeScene* scene)
{
	TOE_RESULT res;

	/* Check if there are messages in queue*/
	if (scene->currentReadPosition == scene->currentWritePosition)
		return 0;

	ToeMessageGetId(scene, &scene->currentInMessageId);
	ToeMessageGetSize(scene, &scene->currentInMessageSize);

	res = ToeRouteMessage(scene,0, toeCoreCallbackTable,sizeof(toeCoreCallbackTable)/sizeof(ToeMessageRoute), ToePorcessOtherMessage);
	if (res == TOE_MESSAGE_IS_NOT_HANDELED)
	{
		//ToeGetMessageProperty(scene,sizeof(scene->currentInMessageId),sizeof(scene->currentInMessageSize),&scene->currentInMessageSize);
	}

	scene->currentReadPosition += scene->currentInMessageSize;
	scene->currentInMessageSize = 0;
	scene->currentInMessageId = 0;

	return (scene->currentReadPosition != scene->currentWritePosition);
}

/**
 * Allocates new message in message buffer. One message could be allocated at one time. 
 * Use ToePostMessage to post current message before allocate new one.
 * @param scene Pointer to TOE scene.
 * @param messageId Message identifier (hash of the message name).
 * @param size Size of the message.
 */
void ToeAllocateMessage(ToeScene* scene, unsigned long messageId, int size)
{
	TOE_ASSERT(scene->currentOutMessageId==0,"Post previous message before allocating new one");
	scene->currentOutMessageId = messageId;
	scene->currentOutMessageSize = size;
	ToeSetMessageProperty(scene,0,sizeof(scene->currentOutMessageId),&scene->currentOutMessageId);
}

/**
 * Posts current message.
 * @param scene Pointer to TOE scene.
 */
void ToePostMessage(ToeScene* scene)
{
	TOE_ASSERT(scene->currentOutMessageId!=0,"Allocate message before calling post");
	ToeSetMessageProperty(scene, sizeof(scene->currentOutMessageId), sizeof(scene->currentOutMessageSize), &scene->currentOutMessageSize);
	scene->currentWritePosition = (scene->currentWritePosition+scene->currentOutMessageSize) & scene->messageBufferMask;
	scene->currentOutMessageId = 0;
	scene->currentOutMessageSize = 0;
}

/**
 * Sets message property value by coping source bytes into message body.
 * @param scene Pointer to TOE scene.
 */
void ToeSetMessageProperty(ToeScene* scene, unsigned long offset, unsigned long size, const void* src)
{
	unsigned long pos = (scene->currentWritePosition + offset) & scene->messageBufferMask;
	const unsigned char* begin = (const unsigned char*)src;
	const unsigned char* end = begin+size;
	while(begin != end)
	{
		scene->messageBuffer[pos] = *begin;
		++begin;
		pos = (pos+1) & scene->messageBufferMask;
	}
}

/**
 * Sets message variable size property value by attaching source bytes to message tail.
 * @param scene Pointer to TOE scene.
 */
void ToeSetMessageVariableSizeProperty(ToeScene* scene, unsigned long offset, unsigned long size, const void* src)
{
	unsigned long pos = (scene->currentWritePosition + offset) & scene->messageBufferMask;
	const unsigned char* begin = (const unsigned char*)src;
	const unsigned char* end = begin+size;
	while(begin != end)
	{
		scene->messageBuffer[pos] = *begin;
		++begin;
		pos = (pos+1) & scene->messageBufferMask;
	}
}

/**
 * Gets message property value by coping bytes from message body.
 * @param scene Pointer to TOE scene.
 */
void ToeGetMessageProperty(const ToeScene* scene, unsigned long offset, unsigned long size, void* dst)
{
	register unsigned long pos = (scene->currentReadPosition + offset) & scene->messageBufferMask;
	unsigned char* begin = (unsigned char*)dst;
	unsigned char* end = begin+size;
	while(begin != end)
	{
		*begin = scene->messageBuffer[pos];
		++begin;
		pos = (pos+1) & scene->messageBufferMask;
	}
}

/**
 * Gets pointer to message property value at message tail.
 * @param scene Pointer to TOE scene.
 */
unsigned long ToeGetMessageVariableSizeProperty(const ToeScene* scene, unsigned long offset, void** dst);

void ToeRegisterCoreMessages(ToeMessageRegistry* reg)
{
	ToeRegisterMessages(reg, toe_coreMessages,1);
}