#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec4 aColor;

out vec4 vertexColor;
out vec4 vertexPos;

uniform vec3 ourTranslate;

void main()
{
    gl_Position = vec4(aPosition + ourTranslate, 1.0);
	vertexPos = gl_Position;
	vertexColor = aColor;
}