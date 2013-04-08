#include "toe.h"

IW_MANAGED_IMPLEMENT(CToeScene);

CToeScene::CToeScene()
{
	scene = 0;
}

CToeScene::~CToeScene()
{
	if (scene != 0)
	{
		ToeDestroyScene(scene);
	}
}

//-------------------------------------------------------------------------------------------------

void CToeScene::Serialise()
{
    CIwResource::Serialise();
}

//-------------------------------------------------------------------------------------------------

void CToeScene::ParseClose(CIwTextParserITX* pParser)
{
}

//-------------------------------------------------------------------------------------------------

ToeScene*CToeScene::GetScene()
{
	if (scene)
		return scene;
	ToeSceneOptions options;
	ToeSetDefaultOptions(&options);
	options.CreateSystemCallback = CreateSystemCallback;
	options.CreateSystemCallbackContext = this;
	scene = ToeCreateScene(&options);
	return scene;
}

TOE_RESULT CToeScene::CreateSystemCallback(unsigned long id, void* context, ToeSceneSystem* system)
{
	void* ptr = IwClassFactoryCreate(id);
	if (ptr == 0)
		return TOE_ERROR;
	CToeSystem* s = (CToeSystem*)ptr;
	system->Id = id;
	system->MessageCallbackContext = s;
	system->MessageCallback = MessageCallback;
	return TOE_SUCCESS;
}
TOE_RESULT  CToeScene::MessageCallback(ToeScene* scene, void* context)
{
	return TOE_ERROR;
}
