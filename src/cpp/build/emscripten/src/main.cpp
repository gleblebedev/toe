#include <math.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#include <GL/gl.h>
#include <GL/glut.h>

static void ToeInit(void)
{
}

static void ToeIdle(void)
{
}

static void ToeReshape(int width, int height)
{
}
static void ToeSpecial(int special, int crap, int morecrap)
{
	switch (special) {
	//case GLUT_KEY_LEFT:
	//	view_rot[1] += 5.0;
	//	break;
	//case GLUT_KEY_RIGHT:
	//	view_rot[1] -= 5.0;
	//	break;
	//case GLUT_KEY_UP:
	//	view_rot[0] += 5.0;
	//	break;
	//case GLUT_KEY_DOWN:
	//	view_rot[0] -= 5.0;
	//	break;
	case GLUT_KEY_F11:
		glutFullScreen();
		break;
	}
}
static void ToeDraw(void)
{
	glClearColor(0.0, 1.0, 1.0, 0.0);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glutSwapBuffers();
}

int main(int argc, char *argv[])
{
   /* Initialize the window */
   glutInit(&argc, argv);
   glutInitWindowSize(300, 300);
   glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB | GLUT_DEPTH);

   glutCreateWindow("es2toe");

   /* Set up glut callback functions */
   glutIdleFunc (ToeIdle);
   glutReshapeFunc(ToeReshape);
   glutDisplayFunc(ToeDraw);
   glutSpecialFunc(ToeSpecial);

   /* Initialize the gears */
   ToeInit();

   glutMainLoop();

   return 0;
}
