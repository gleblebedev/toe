// Include the single header file for the IwGx module
#include "IwGx.h"
#include "IwGraphics.h"
#include "IwMaterial.h"
#include "IwModel.h"
#include "IwAnim.h"
#include "IwAnimSkel.h"
#include "IwTexture.h"
#include "s3eFile.h"
#include "s3eHook.h"

S3E_API typedef s3eFile* (*FileOpenPtr)(const char* filename, const char* mode);
S3E_API typedef uint32 (*FileWritePtr)(const void* buffer, uint32 elemSize, uint32 noElems, s3eFile* file);
S3E_API typedef s3eResult (*FileSeekPtr)(s3eFile* file, int32 offset, s3eFileSeekOrigin origin);
S3E_API typedef s3eResult (*FileClosePtr)(s3eFile* file);

FileOpenPtr originalFileOpen;
FileWritePtr originalFileWrite;
FileSeekPtr originalFileSeek;
FileClosePtr originalFileClose;
s3eFile* myLog = 0;
s3eFile* watchFor = 0;
void myFileLog(const void* buf, int l)
{
	if (myLog == 0)
		myLog = originalFileOpen("io.log","w");
	originalFileWrite(buf,1,l,myLog);
	//originalFileClose(log);
}

S3E_API s3eFile* myFileOpen(const char* filename, const char* mode)
{
	s3eFile* res = originalFileOpen(filename, mode);

	int len = strlen(filename);
	if (filename[len-3] == 'n' && filename[len-2] == 'n' && filename[len-1] == 'n')
		watchFor = res;

	static char buf[4096];
	sprintf(buf, "\nopen %s 0x%08X\n\n",filename,res);
	myFileLog(buf, strlen(buf));

	return res;
}
S3E_API uint32 myFileWrite(const void* buffer, uint32 elemSize, uint32 noElems, s3eFile* file)
{
	if (watchFor != 0 && watchFor == file)
		return originalFileWrite(buffer, elemSize,noElems,file);
		
	static char hex[16] = {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'};
	static char buf[4096];
	sprintf(buf, "write to 0x%08X at %d from 0x%08X (%d), %d bytes %d times: ",file,s3eFileTell(file),buffer,buffer, elemSize,noElems);
	int l = strlen(buf);
	const unsigned char* src = (const unsigned char* )buffer;
	for (uint32 i=0; i<noElems; ++i)
	{
		buf[l++] = ' ';
		if (elemSize == 4)
		{
			uint32 v = *((unsigned int*)src);
			double vd = *((float*)src);
			sprintf(buf+l,"0x%08X (%d/%g)",v,v,vd);
			l += strlen(buf+l);
			src+=4;
		}
		else
		{
			for (uint32 j=0; j<elemSize; ++j)
			{
				buf[l++] = hex[(*src) >> 4];
				buf[l++] = hex[(*src) & 15];
				++src;

				if (l > 4000)
				{
					buf[l++] = '.';
					buf[l++] = '.';
					buf[l++] = '.';
					goto endline;
				}
			}
		}
		if (l > 4000)
		{
			buf[l++] = '.';
			buf[l++] = '.';
			buf[l++] = '.';
			goto endline;
		}
	}
	endline:
	buf[l++] = 0x0D;
	buf[l++] = 0x0A;
	buf[l] = '\0';
	myFileLog(buf,l);
	return originalFileWrite(buffer, elemSize,noElems,file);
}
S3E_API s3eResult myFileSeek(s3eFile* file, int32 offset, s3eFileSeekOrigin origin)
{
	static char buf[256];
	int32 pos = s3eFileTell(file);
	switch (origin)
	{
	case S3E_FILESEEK_SET:
		pos = offset;
		break;
	case S3E_FILESEEK_CUR:
		pos = s3eFileTell(file)+offset;
		break;
	case S3E_FILESEEK_END:
		pos = -1;
		break;
	}
	sprintf(buf, "seek 0x%08X by %d (origin %s) to %d\n", file, offset, (origin==S3E_FILESEEK_SET)?"S3E_FILESEEK_SET":((origin==S3E_FILESEEK_CUR)?"S3E_FILESEEK_CUR":"S3E_FILESEEK_END"), pos);
	myFileLog(buf, strlen(buf));

	return originalFileSeek(file, offset,origin);
}
S3E_API s3eResult myFileClose(s3eFile* file)
{
	static char buf[256];
	sprintf(buf, "\nclose 0x%08X\n\n", file);
	myFileLog(buf, strlen(buf));

	return originalFileClose(file);
}

void LogFileOperations()
{
	static void* pointers[64];
	int i=0;
	pointers[i++] = &s3eFileOpen;
	pointers[i++] = &myFileOpen;
	pointers[i++] = 0;

	pointers[i++] = &s3eFileWrite;
	pointers[i++] = &myFileWrite;
	pointers[i++] = 0;

	pointers[i++] = &s3eFileSeek;
	pointers[i++] = &myFileSeek;
	pointers[i++] = 0;

	pointers[i++] = &s3eFileClose;
	pointers[i++] = &myFileClose;
	pointers[i++] = 0;

	pointers[i++] = 0;
	s3eResult res = HookOnFileOperations(pointers);
	originalFileOpen = (FileOpenPtr)pointers[2];
	originalFileWrite = (FileWritePtr)pointers[3+2];
	originalFileSeek = (FileSeekPtr)pointers[6+2];
	originalFileClose = (FileClosePtr)pointers[9+2];
}

// Standard C-style entry point. This can take args if required.
int main()
{
    // Initialise the IwGx drawing module
    IwGxInit();
	IwResManagerInit();
    IwGraphicsInit();
	IwAnimInit();

	LogFileOperations();
	
    // Set the background colour to (opaque) blue
    IwGxSetColClear(0, 0, 0, 0xff);

	CIwResGroup* g = IwGetResManager()->LoadGroup("male_lod0.group");
	CIwResList* texures = g->GetListNamed("CIwAnimSkin");
	CIwAnimSkin* t = (CIwAnimSkin*)texures->m_Resources[0];
	CIwModel* head = (CIwModel*)g->GetResNamed("male_head_0_lod0","CIwModel");
	CIwModel* torso = (CIwModel*)g->GetResNamed("male_torse_jacket0_lod0","CIwModel");
	CIwModel* legs = (CIwModel*)g->GetResNamed("male_legs_trousers0_lod0","CIwModel");
	CIwAnimSkin* headSkin = (CIwAnimSkin*)g->GetResNamed("male_head_0_lod0","CIwAnimSkin");
	CIwAnimSkin* torsoSkin = (CIwAnimSkin*)g->GetResNamed("male_torse_jacket0_lod0","CIwAnimSkin");
	CIwAnimSkin* legsSkin = (CIwAnimSkin*)g->GetResNamed("male_legs_trousers0_lod0","CIwAnimSkin");

	CIwAnimSkel* skel = (CIwAnimSkel*)g->GetResNamed("male_skel_lod0","CIwAnimSkel");
	CIwAnimPlayer* player = new CIwAnimPlayer;
    player->SetSkel(skel);
	CIwMaterial* mat  = new CIwMaterial();
    // Loop forever, until the user or the OS performs some action to quit the app
    while (!s3eDeviceCheckQuitRequest())
    {
        // Clear the surface
        IwGxClear();

		// Set field of view
		IwGxSetPerspMul(IwGxGetScreenWidth() / 1.0f);

		// Set near and far planes
	    IwGxSetFarZNearZ(0x4010, 0x10);

        // Use the built-in font to display a string at coordinate (120, 150)
        //IwGxPrintString(120, 150, "Hello, World!");
		

		CIwFMat m;
		float k = 1.5f;
		//CIwVec3 myPos = CIwVec3(IW_GEOM_ONE*k,-IW_GEOM_ONE*k,IW_GEOM_ONE*k);
		CIwFVec3 myPos = CIwFVec3(512*k, 64*k, 1024*k);
		//m.LookAt(myPos,CIwVec3(0,0,IW_GEOM_ONE*k),CIwVec3(0,0,-IW_GEOM_ONE));
		m.LookAt(myPos,CIwFVec3(0, 0, 0),-CIwFVec3::g_AxisZ);
	    m.t = myPos;
		CIwFMat fm(m);
		IwGxSetViewMatrix(&fm);

	
		//IwGxSetVertStream(0,0);
		

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

			IwGxSetMaterial(mat);
		static CIwFVec3 verts[6] = {CIwFVec3(0,0,0),CIwFVec3(1000,0,0),CIwFVec3(0,0,0),CIwFVec3(0,1000,0),CIwFVec3(0,0,0),CIwFVec3(0,0,1000)};
		IwGxSetVertStream(verts,6);

		static CIwColour cols[6];
		cols[0].Set(255,0,0,255);
		cols[1].Set(255,0,0,255);
		cols[2].Set(0,255,0,255);
		cols[3].Set(0,255,0,255);
		cols[4].Set(0,0,255,255);
		cols[5].Set(0,0,255,255);
		IwGxSetColStream(cols,6);
		static uint16 indices [6] = {0,1,2,3,4,5};
		IwGxDrawPrims(IW_GX_LINE_LIST, indices,6);

        // Standard EGL-style flush of drawing to the surface
        IwGxFlush();

        // Standard EGL-style flipping of double-buffers
        IwGxSwapBuffers();

        // Sleep for 0ms to allow the OS to process events etc.
        s3eDeviceYield(0);
    }
	delete mat;
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
