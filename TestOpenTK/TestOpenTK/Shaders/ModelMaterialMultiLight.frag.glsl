﻿#version 330 core

struct Material {
    //vec3 ambient;
    sampler2D diffuse;
    sampler2D specular;
	sampler2D emission;
    float shininess;
}; 

struct DirLight {
    vec3  direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct PointLight {
	vec4 position;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float constant;
	float linear;
	float quadratic;
};

struct SpotLight {
	vec4 position;
	vec3  direction;
	float cutOff;
	float outerCutOff;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float constant;
	float linear;
	float quadratic;
};

//uniform vec3 lightColor;
uniform Material material;

uniform DirLight dirLight;

#define NR_POINT_LIGHTS 4
uniform PointLight pointLights[NR_POINT_LIGHTS];

uniform SpotLight spotLight;


out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec2 FragUV;


vec3 CalcDirLight(DirLight light, vec3 norm, vec3 viewDir)
{
	// ambient
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, FragUV));

    // diffuse

	vec3 lightDir = normalize(-light.direction);
	
    float diff = max(dot(norm, lightDir), 0.0);

    vec3 diffuse = light.diffuse * (diff * vec3(texture(material.diffuse, FragUV)));

    // specular
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specColor = vec3(texture(material.specular, FragUV));

    vec3 specular = light.specular * (spec * specColor);  

    vec3 result = ambient + diffuse + specular;

	//return specular;
	return result;
}

vec3 CalcPointLight(PointLight light, vec3 norm, vec3 fragPos, vec3 viewDir)
{
	vec4 lightPos = light.position;

	// ambient
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, FragUV));

    // diffuse

	vec3 lightDir = normalize(lightPos.xyz - fragPos);
    float distance = length(lightPos.xyz - fragPos);
	float attenuation = 1.0 / (light.constant + light.linear * distance + 
			light.quadratic * (distance * distance));
		
    float diff = max(dot(norm, lightDir), 0.0);

    vec3 diffuse = light.diffuse * (diff * vec3(texture(material.diffuse, FragUV)));

    // specular
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specColor = vec3(texture(material.specular, FragUV));

    vec3 specular = light.specular * (spec * specColor);  

	ambient  *= attenuation;  
    diffuse   *= attenuation;
    specular *= attenuation;

    vec3 result = ambient + diffuse + specular;
    return result;
}

vec3 CalcSpotLight(SpotLight light, vec3 norm, vec3 fragPos, vec3 viewDir)
{
	vec4 lightPos = light.position;

	// ambient
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, FragUV));

    // diffuse
	vec3 lightDir = normalize(lightPos.xyz - fragPos);

	float distance = length(lightPos.xyz - fragPos);
	float attenuation = 1.0 / (light.constant + light.linear * distance + 
		light.quadratic * (distance * distance));

    float theta     = dot(lightDir, normalize(-light.direction));
    float epsilon   = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

		
    float diff = max(dot(norm, lightDir), 0.0);

    vec3 diffuse = light.diffuse * (diff * vec3(texture(material.diffuse, FragUV)));

    // specular
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specColor = vec3(texture(material.specular, FragUV));

    vec3 specular = light.specular * (spec * specColor);  

	ambient  *= attenuation;  
    diffuse   *= attenuation * intensity;
    specular *= attenuation * intensity;

    vec3 result = ambient + diffuse + specular;
    return result;
}

void main()
{

	vec3 result = vec3(0);

	vec3 norm = normalize(Normal);
	vec3 viewPos = vec3(0);
    vec3 viewDir = normalize(viewPos - FragPos);

	result += CalcDirLight(dirLight, norm, viewDir);

	FragColor = vec4(result, 1.0);

	for(int i = 0; i < NR_POINT_LIGHTS; i++)
        result += CalcPointLight(pointLights[i], norm, FragPos, viewDir);    
    
    result += CalcSpotLight(spotLight, norm, FragPos, viewDir);    

	//result += = texture(material.emission, FragUV).rgb;

    //FragColor = vec4(result, 1.0);
}