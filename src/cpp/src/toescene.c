#include <stdlib.h>
#include "../h/toescene.h"

ToeScene* ToeCreateScene(const ToeSceneOptions* options)
{
	ToeScene* scene = (ToeScene*)malloc(sizeof(ToeScene));
	scene->totalLayers = 0;
	scene->totalSystems = 0;
	scene->options = *options;
	return scene;
}
void ToeDestroyScene(ToeScene*scene)
{
	free(scene);
}