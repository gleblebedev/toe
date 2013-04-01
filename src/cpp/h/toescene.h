#ifndef TOE_SCENE
#define TOE_SCENE

#ifdef  __cplusplus
extern "C" {
#endif

/*
Toe Scene Layer
*/
typedef struct ToeSceneSystem
{
	uint Id;
} ToeSceneSystem;

/*
Toe Scene Layer
*/
typedef struct ToeSceneLayer
{
	uint Id;
} ToeSceneLayer;

/*
Toe system factory callback
*/
typedef void (*ToeCreateSystemCallback)(uint id, void* context, ToeSceneSystem* system);

/*
Toe Scene Options
*/
typedef struct ToeSceneOptions
{
	ToeCreateSystemCallback CreateSystemCallback;
	void* CreateSystemCallbackContext;
} ToeSceneOptions;

/*
Toe Scene
*/
typedef struct ToeScene
{
	int totalSystems;
	int totalLayers;
	ToeSceneOptions	options;

	ToeSceneLayer systems [32];
	ToeSceneLayer layers [32];
} ToeScene;

/*
Create new scene.
*/
ToeScene* ToeCreateScene(const ToeSceneOptions*);

/*
Destroy scene.
*/
void ToeDestroyScene(ToeScene*);

#ifdef  __cplusplus
}
#endif

#endif
