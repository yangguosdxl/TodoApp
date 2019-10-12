#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec3 aColor;

out vec3 Normal;
out vec3 FragPos;  
out vec3 Color;

uniform mat4 ViewToProject;
uniform mat4 WorldToView;
uniform mat4 ModelToWorld;

void main()
{
	gl_Position = ViewToProject * WorldToView * ModelToWorld * vec4(aPosition, 1.0);
	FragPos = (ModelToWorld * vec4(aPosition, 1.0)).xyz;
	Normal = (ModelToWorld * vec4(aNormal, 0.0)).xyz;
	Color = aColor;
}