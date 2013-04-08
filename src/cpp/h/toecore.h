#ifndef TOE_SCENE
#define TOE_SCENE

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

#ifdef  __cplusplus
extern "C" {
#endif

typedef int TOE_RESULT;
#define TOE_SUCCESS ((TOE_RESULT)1)
#define TOE_ERROR ((TOE_RESULT)0)

#define TOE_ASSERT(a,b)

struct ToeScene_t;

typedef struct ToeScene_t ToeScene;

unsigned long ToeHashString(const char* string);

/*
Toe system factory callback
*/
typedef TOE_RESULT (*ToeMessageCallback)(ToeScene* scene, void* context);

/*
Toe Scene Layer
*/
typedef struct ToeSceneSystem
{
	unsigned long Id;
	void* MessageCallbackContext;
	ToeMessageCallback MessageCallback;

} ToeSceneSystem;

typedef struct ToeMessageCallbackTableItem
{
	unsigned long MessageId;
	ToeMessageCallback Callback;
} ToeMessageCallbackTableItem;

void ToeSortMessageCallbackTable (ToeMessageCallbackTableItem* table, unsigned long size);

TOE_RESULT ToeLookupMessageCallbackTable (const ToeMessageCallbackTableItem* table, unsigned long size);

/*
Toe Scene Layer
*/
typedef struct ToeSceneLayer
{
	unsigned long Id;
} ToeSceneLayer;

/*
Toe system factory callback
*/
typedef TOE_RESULT (*ToeCreateSystemCallback)(unsigned long id, void* context, ToeSceneSystem* system);

/*
Toe Scene Options
*/
typedef struct ToeSceneOptions
{
	ToeCreateSystemCallback CreateSystemCallback;
	void* CreateSystemCallbackContext;
	unsigned long MessageBufferSize;
} ToeSceneOptions;

void ToeSetDefaultOptions(ToeSceneOptions* options);

/**
 * Allocates new scene.
 * @param options Pointer to TOE scene options container.
 * @return Pointer to allocated scene.
 */
ToeScene* ToeCreateScene(const ToeSceneOptions* options);

/**
 * Destroyes the scene.
 * @param scene Pointer to the scene to destroy.
 */
void ToeDestroyScene(ToeScene* scene);

/**
 * Process next message in message buffer.
 * @param scene Pointer to the scene to destroy.
 */
int ToePorcessMessage(ToeScene* scene);

/**
 * Allocates new message in message buffer. One message could be allocated at one time. 
 * Use ToePostMessage to post current message before allocate new one.
 * @param scene Pointer to TOE scene.
 * @param messageId Message identifier (hash of the message name).
 * @param size Size of the message.
 */
void ToeAllocateMessage(ToeScene* scene, unsigned long messageId, int size);

/**
 * Posts current message.
 * @param scene Pointer to TOE scene.
 */
void ToePostMessage(ToeScene* scene);

/**
 * Sets message property value by coping source bytes into message body.
 * @param scene Pointer to TOE scene.
 */
void ToeSetMessageProperty(ToeScene* scene, unsigned long offset, unsigned long size, const void* src);

//inline void ToeSetUIntMessageProperty(ToeScene* scene, unsigned long offset, const unsigned long src)
//{
//	ToeSetMessageProperty(scene, offset, sizeof(unsigned long), &src);
//}

/**
 * Sets message variable size property value by attaching source bytes to message tail.
 * @param scene Pointer to TOE scene.
 */
void ToeSetVariableSizeMessageProperty(ToeScene* scene, unsigned long offset, unsigned long size, const void* src);

/**
 * Gets message property value by coping bytes from message body.
 * @param scene Pointer to TOE scene.
 */
void ToeGetMessageProperty(const ToeScene* scene, unsigned long offset, unsigned long size, void* dst);

/**
 * Gets pointer to message property value at message tail.
 * @param scene Pointer to TOE scene.
 */
unsigned long ToeGetVariableSizeMessageProperty(const ToeScene* scene, unsigned long offset, void** dst);

typedef struct ToeMessage
{
	ToeScene* scene;
} ToeMessage;

typedef struct ToeCreateSystemMessage
{
	union {
	ToeScene* scene;
	ToeMessage message;
	};
} ToeCreateSystemMessage;

typedef struct ToeCreateLayerMessage
{
	ToeScene* scene;
} ToeCreateLayerMessage;

#ifdef  __cplusplus
}
#endif

#endif
