#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec4 aColor;
layout (location = 2) in vec2 aST;

out vec4 vertexColor;
out vec4 vertexPos;
out vec2 texCoord;

uniform vec3 ourTranslate;

void main()
{
    gl_Position = vec4(aPosition + ourTranslate, 1.0);
	vertexPos = gl_Position;
	vertexColor = aColor;
	texCoord = aST;
}