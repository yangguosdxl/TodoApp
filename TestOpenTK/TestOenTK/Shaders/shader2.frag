#version 330 core

in vec4 vertexColor;
in vec4 vertexPos;

out vec4 FragColor;

uniform vec4 ourColor;

void main()
{
    FragColor = vertexPos + ourColor;
}