#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aUV;

out vec3 Normal;
out vec3 FragPos;  
out vec2 FragUV;

uniform mat4 ViewToProject;
uniform mat4 WorldToView;
uniform mat4 ModelToWorld;

void main()
{
	gl_Position = ViewToProject * WorldToView * ModelToWorld * vec4(aPosition, 1.0);
	FragPos = (WorldToView * ModelToWorld * vec4(aPosition, 1.0)).xyz;
	Normal = (WorldToView * ModelToWorld * vec4(aNormal, 0.0)).xyz;
	FragUV = aUV;
}