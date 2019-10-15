#version 330 core

struct Material {
    //vec3 ambient;
    sampler2D diffuse;
    sampler2D specular;
	sampler2D emission;
    float shininess;
}; 

struct Light {
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
uniform Light light;

out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec3 Color;
in vec2 FragUV;





void main()
{
	vec4 lightPos = light.position;

	// ambient
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, FragUV));

    // diffuse
    vec3 norm = normalize(Normal);

	vec3 lightDir = normalize(-lightPos.xyz);
	float attenuation = 1.0;
    float intensity = 1;
	if (lightPos.w == 1.0)
    {
		lightDir = normalize(lightPos.xyz - FragPos);

		float distance = length(lightPos.xyz - FragPos);
		attenuation = 1.0 / (light.constant + light.linear * distance + 
			light.quadratic * (distance * distance));

        if (light.cutOff > 0)
        {
            float theta     = dot(lightDir, normalize(-light.direction));
            float epsilon   = light.cutOff - light.outerCutOff;
            intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);
        }
		
	}
		
    float diff = max(dot(norm, lightDir), 0.0);

    vec3 diffuse = light.diffuse * (diff * vec3(texture(material.diffuse, FragUV)));

    // specular
	vec3 viewPos = vec3(0);
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specColor = vec3(texture(material.specular, FragUV));

    vec3 specular = light.specular * (spec * specColor);  

	vec3 emission = texture(material.emission, FragUV).rgb;

	ambient  *= attenuation;  
    diffuse   *= attenuation * intensity;
    specular *= attenuation * intensity;
	emission *= attenuation;

    vec3 result = ambient + diffuse + specular + emission;
    FragColor = vec4(result, 1.0);
}