#ifndef TOE_MARMALADE_SDK
#define TOE_MARMALADE_SDK

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

#include <IwManagedList.h>
#include <IwResource.h>

#include <toecore.h>
#include <toemsgreg.h>

/**
 * Initialises Toe.
 */
void ToeInit();

/**
 * Returns global message registry.
 */
ToeMessageRegistry* ToeGetMessageRegistry();

/**
 * Shuts down Toe.
 */
void ToeTerminate();

class CToeScene: public CIwResource
{
	ToeScene* scene;
public:
	IW_MANAGED_DECLARE_NOCOPY(CToeScene);

    /**
     * Constructor.
     */
    CToeScene();

    /**
     * Destructor.
     */
    virtual ~CToeScene();

	// CIwManaged virtuals
    virtual void Serialise();
    virtual void ParseClose(CIwTextParserITX* pParser);

	ToeScene*GetScene();
protected:
	
	static TOE_RESULT  CreateSystemCallback(unsigned long id, void* context, ToeSceneSystem* system);
	static TOE_RESULT  MessageCallback(ToeScene* scene, void* context);
};

class CToeSystem: public CIwManaged
{
	public:
	IW_MANAGED_DECLARE_NOCOPY(CToeSystem);

	   /**
     * Constructor.
     */
    CToeSystem();

    /**
     * Destructor.
     */
    virtual ~CToeSystem();
};

#endif