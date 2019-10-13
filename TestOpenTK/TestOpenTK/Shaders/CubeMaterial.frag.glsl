#version 330 core

in vec3 Normal;
in vec3 FragPos;
in vec3 Color;
out vec3 FragColor;

//uniform vec3 objectColor;
uniform vec3 lightColor;
uniform vec3 lightPos;
//uniform vec3 viewPos;

void main()
{
	vec3 viewPos = vec3(0);
	// ambient
	float ambientStrength = 0.3;
    vec3 ambient = ambientStrength * lightColor;

	// diffuse
	float diffuseStrength = 0.5;
	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(lightPos - FragPos);

	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diffuseStrength* diff * lightColor;

	// specular
	float specularStrength = 10;
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);

	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	vec3 specular = specularStrength * spec * lightColor;

	//FragColor = vec3(diff);

	//FragColor = Color * (ambient  + specular);

    FragColor = Color * (ambient + diffuse + specular);
}