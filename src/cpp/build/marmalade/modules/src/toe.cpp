#include "toe.h"
#include <toecoremsgs.h>

ToeMessageRegistry* toe_global_message_registry = 0;
int	toe_init_counter = 0;

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

void ToeInit()
{
	if (!toe_init_counter)
	{
		toe_global_message_registry = ToeCreateMessageRegistry(1024);
		ToeRegisterCoreMessages(toe_global_message_registry);
	}
	++toe_init_counter;
}

ToeMessageRegistry* ToeGetMessageRegistry()
{
	return toe_global_message_registry;
}

void ToeTerminate()
{
	--toe_init_counter;
	if (!toe_init_counter)
	{
		ToeDestroyMessageRegistry(toe_global_message_registry);
	}
}