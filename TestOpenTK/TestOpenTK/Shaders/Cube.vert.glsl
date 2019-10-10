#version 330 core
layout (location = 0) in vec3 aPosition;

uniform mat4 ViewToProject;
uniform mat4 WorldToView;
uniform mat4 ModelToWorld;

void main()
{
	gl_Position = ViewToProject * WorldToView * ModelToWorld * vec4(aPosition, 1.0);
}