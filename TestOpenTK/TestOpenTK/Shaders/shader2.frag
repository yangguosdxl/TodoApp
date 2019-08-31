#version 330 core

in vec4 vertexColor;
in vec4 vertexPos;
in vec2 texCoord;

out vec4 FragColor;

uniform vec4 ourColor;
uniform sampler2D ourTexture;
uniform sampler2D ourTexture2;

void main()
{
    FragColor = ourColor + mix(texture(ourTexture, texCoord), texture(ourTexture2, texCoord), 0.8);
}