#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;

uniform mat4 ViewToProject;
uniform mat4 WorldToView;
uniform mat4 ModelToWorld;

void main()
{
	gl_Position = ViewToProject * WorldToView * ModelToWorld * vec4(aPosition, 1.0);
}