#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec3 aColor;

out vec3 Color;

uniform mat4 ViewToProject;
uniform mat4 WorldToView;
uniform mat4 ModelToWorld;

uniform vec3 lightColor;
uniform vec3 lightPos;

void main()
{
	gl_Position = ViewToProject * WorldToView * ModelToWorld * vec4(aPosition, 1.0);
	vec3 FragPos = (WorldToView * ModelToWorld * vec4(aPosition, 1.0)).xyz;
	vec3 Normal = (WorldToView * ModelToWorld * vec4(aNormal, 0.0)).xyz;

	vec3 viewPos = vec3(0);
	// ambient
	float ambientStrength = 0.3;
	vec3 ambient = ambientStrength * lightColor;

	// diffuse
	float diffuseStrength = 0.5;
	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(lightPos - FragPos);

	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diffuseStrength * diff * lightColor;

	// specular
	float specularStrength = 10;
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);

	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	vec3 specular = specularStrength * spec * lightColor;

	//FragColor = vec3(diff);

	//FragColor = Color * (ambient  + specular);

	Color = aColor * (ambient + diffuse + specular);

}