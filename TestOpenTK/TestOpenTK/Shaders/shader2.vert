#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec4 aColor;
layout (location = 2) in vec2 aST;

out vec4 vertexColor;
out vec4 vertexPos;
out vec2 texCoord;

uniform mat4 pvm;

void main()
{
    gl_Position = pvm * vec4(aPosition, 1.0) ; // * model * view * proj;
	vertexPos = gl_Position;
	vertexColor = aColor;
	texCoord = aST;
}