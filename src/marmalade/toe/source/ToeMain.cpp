// Include the single header file for the IwGx module
#include "IwGx.h"
#include "IwGraphics.h"
#include "IwMaterial.h"
#include "IwModel.h"
#include "IwAnim.h"
#include "IwAnimSkel.h"
#include "IwTexture.h"

// Standard C-style entry point. This can take args if required.
int main()
{
    // Initialise the IwGx drawing module
    IwGxInit();
	IwResManagerInit();
    IwGraphicsInit();
	IwAnimInit();
	
    // Set the background colour to (opaque) blue
    IwGxSetColClear(0, 0, 0xff, 0xff);

	CIwResGroup* g = IwGetResManager()->LoadGroup("male_lod0.group");

	CIwModel* head = (CIwModel*)g->GetResNamed("male_head_0_lod0","CIwModel");
	CIwModel* torso = (CIwModel*)g->GetResNamed("male_torse_jacket0_lod0","CIwModel");
	CIwModel* legs = (CIwModel*)g->GetResNamed("male_legs_trousers0_lod0","CIwModel");
	CIwAnimSkin* headSkin = (CIwAnimSkin*)g->GetResNamed("male_head_0_lod0","CIwAnimSkin");
	CIwAnimSkin* torsoSkin = (CIwAnimSkin*)g->GetResNamed("male_torse_jacket0_lod0","CIwAnimSkin");
	CIwAnimSkin* legsSkin = (CIwAnimSkin*)g->GetResNamed("male_legs_trousers0_lod0","CIwAnimSkin");

	CIwAnimSkel* skel = (CIwAnimSkel*)g->GetResNamed("male_skel_lod0","CIwAnimSkel");
	CIwAnimPlayer* player = new CIwAnimPlayer;
    player->SetSkel(skel);

    // Loop forever, until the user or the OS performs some action to quit the app
    while (!s3eDeviceCheckQuitRequest())
    {
        // Clear the surface
        IwGxClear();

		// Set field of view
		IwGxSetPerspMul(IwGxGetScreenWidth() / 1.0f);

		// Set near and far planes
	    //IwGxSetFarZNearZ(0x4010, 0x10);

        // Use the built-in font to display a string at coordinate (120, 150)
        IwGxPrintString(120, 150, "Hello, World!");



		CIwMat m;
		int k = 512;
		CIwVec3 myPos = CIwVec3(IW_GEOM_ONE*k,-IW_GEOM_ONE*k,IW_GEOM_ONE*k);
		m.LookAt(myPos,CIwVec3(0,0,IW_GEOM_ONE*k),CIwVec3(0,0,-IW_GEOM_ONE));
	    m.t = myPos;
		CIwFMat fm(m);
		
		IwGxSetViewMatrix(&fm);

		CIwFMat fmodel;
		fmodel.SetIdentity();
		IwGxSetModelMatrix(&fmodel);

		IwAnimSetSkelContext(player->GetSkel());
		IwAnimSetSkinContext(headSkin);
		head->Render();
		IwAnimSetSkinContext(torsoSkin);
		torso->Render();
		IwAnimSetSkinContext(legsSkin);
		legs->Render();

	    IwAnimSetSkelContext(NULL);
	    IwAnimSetSkinContext(NULL);

        // Standard EGL-style flush of drawing to the surface
        IwGxFlush();

        // Standard EGL-style flipping of double-buffers
        IwGxSwapBuffers();

        // Sleep for 0ms to allow the OS to process events etc.
        s3eDeviceYield(0);
    }
	delete player;
	IwGetResManager()->DestroyGroup(g);

	IwAnimTerminate();
    IwGraphicsTerminate();
	IwResManagerTerminate();
    // Shut down the IwGx drawing module
    IwGxTerminate();

    // Return
    return 0;
}
