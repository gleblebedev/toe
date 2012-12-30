// Include the single header file for the IwGx module
#include "IwGx.h"
#include "IwGraphics.h"
#include "IwMaterial.h"
#include "IwAnim.h"
#include "IwAnimSkel.h"
#include "IwTexture.h"

// Standard C-style entry point. This can take args if required.
int main()
{
    // Initialise the IwGx drawing module
    IwGxInit();
    IwGraphicsInit();
	IwAnimInit();
    // Set the background colour to (opaque) blue
    IwGxSetColClear(0, 0, 0xff, 0xff);

	CIwResGroup* g = IwGetResManager()->LoadGroup("male_skel_lod0.group");
	CIwAnimSkel* skel = (CIwAnimSkel*)g->GetResNamed("male_skel_lod0","CIwAnimSkel");
	CIwAnimPlayer* player = new CIwAnimPlayer;
    player->SetSkel(skel);
	player->UpdateMatrices();
	const CIwFMat*  m0 = player->GetSkel()->GetBoneFromID(0)->GetMat();
	const CIwFMat*  m1 = player->GetSkel()->GetBoneFromID(1)->GetMat();
	const CIwFMat*  m2 = player->GetSkel()->GetBoneFromID(2)->GetMat();

	delete player;

	IwGetResManager()->DestroyGroup(g);

    // Loop forever, until the user or the OS performs some action to quit the app
    while (!s3eDeviceCheckQuitRequest())
    {
        // Clear the surface
        IwGxClear();

        // Use the built-in font to display a string at coordinate (120, 150)
        IwGxPrintString(120, 150, "Hello, World!");

        // Standard EGL-style flush of drawing to the surface
        IwGxFlush();

        // Standard EGL-style flipping of double-buffers
        IwGxSwapBuffers();

        // Sleep for 0ms to allow the OS to process events etc.
        s3eDeviceYield(0);
    }
	IwAnimTerminate();
    IwGraphicsTerminate();
    // Shut down the IwGx drawing module
    IwGxTerminate();

    // Return
    return 0;
}
