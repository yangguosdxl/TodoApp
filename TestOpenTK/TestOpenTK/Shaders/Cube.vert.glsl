#version 330 core
layout (location = 0) in vec3 aPosition;

uniform mat4 pvm;

void main()
{
    gl_Position = pvm * vec4(aPosition, 1.0);
}