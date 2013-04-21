#ifndef TOE_MESSAGE_REGISTRY_H
#define TOE_MESSAGE_REGISTRY_H

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

#include "toecore.h"

#ifdef  __cplusplus
extern "C" {
#endif

struct ToeMessageRegistry;

typedef struct ToeMessageRegistry ToeMessageRegistry;

/*
Toe Message field descriptor.
Lifetime of pointer to this descriptor should be at least the same as life time of message registry.
Message registry doesn't copy this information, it stores pointer to it.
*/
struct ToeMessageField
{
	/* Name */
	const char* Name;

	/* Type ID */
	unsigned long Type;

	/* Number of values, 1 for scalar, 3 for vector(x,y,z), etc. */
	unsigned long Length;

	/* Name hash as unique field identifier. Calculated by Toe Message Registry. */
	unsigned long Id;

	/* Size of the field in bytes. Calculated by Toe Message Registry. */
	unsigned long Size;

	/* Offset from message start point in bytes. Calculated by Toe Message Registry. */
	unsigned long Offset;
};

typedef struct ToeMessageField ToeMessageField;


/*
Toe Message type descriptor.
Lifetime of pointer to this descriptor should be at least the same as life time of message registry.
Message registry doesn't copy this information, it stores pointer to it.
*/
struct ToeMessage
{
	unsigned long ParentId;
	const char* Name;
	unsigned long NumFields;
	ToeMessageField* Fields;

	/* Message ID. Calculated by Toe Message Registry. */
	unsigned long Id;

	/* Pointer to parent message. Calculated by Toe Message Registry. */
	struct ToeMessage* Parent;

	/* Initial message size in bytes. Calculated by Toe Message Registry. */
	unsigned long InitialSize;
};

typedef struct ToeMessage ToeMessage;

typedef int TOE_REG_RESULT;
enum ToeRegResults
{
	TOE_REG_SUCESS,
	TOE_REG_PARENT_NOT_FOUND,
	TOE_REG_DIFFERENT_MESSAGE_ALREADY_REGISTERED
};

/*
Toe Message Route.
Single record that stores information on where to jump if message was received.
*/
struct ToeMessageRoute
{
	/* Message name */
	const char* MessageName;

	/* Where to jump if the message was received */
	ToeMessageCallback	Callback;

	/* Message ID (name hash) */
	unsigned long Id;
};
typedef struct ToeMessageRoute ToeMessageRoute;

ToeMessageRegistry* ToeCreateMessageRegistry(unsigned long maxNumMessages);

void ToeDestroyMessageRegistry(ToeMessageRegistry* reg);

TOE_REG_RESULT ToeRegisterMessage(ToeMessageRegistry* reg, ToeMessage* message);

TOE_REG_RESULT ToeRegisterMessages(ToeMessageRegistry* reg, ToeMessage* message, unsigned int count);

ToeMessage* ToeFindMessage(ToeMessageRegistry* reg, unsigned long messageId);

/*
Preparing routing table.
Sorts the table.
*/
TOE_MESSAGE_RESULT ToePrapareMessageRoutingTable(ToeMessageRoute* table, unsigned long numRoutes);

/*
Calls apropriate callback or default one if no callback were found or if the message was not handeled by callback.
*/
TOE_MESSAGE_RESULT ToeRouteMessage(ToeScene* scene, void* context, const ToeMessageRoute* table, unsigned long numRoutes, ToeMessageCallback defaultCallback);

#ifdef  __cplusplus
}
#endif

#endif