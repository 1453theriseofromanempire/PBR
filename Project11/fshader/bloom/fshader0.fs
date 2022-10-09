#version 330 core

layout (location = 0) out vec4 FragColor;
layout (locaiton = 1) out vec4 BrightColor;

in VS_OUT{
    vec3 FragPos;
    vec3 Normal;
    vec2 TexCoords;
} fs_in;

struct Light{
    vec3 Position;
    vec3 color;
};

uniform Light lights[4];
uniform sampler2D diffuseMap;
uniform vec3 viwePos;

void main(){
    vec3 color = texture(diffuseMap, fs_in.TexCoords);
    vec3 normal = normalize(fs_in.Normal);

    vec3 ambient = 0.0 * color;

    vec3 lighting = vec3(0.0);
    vec3 viewDir = normalize(viewPos - fs_in.FragPos);

    for(int n = 0; n < 4; n++){
        vec3 lightDir = normalize(lights[n].Position  - fs_in.FragPos);
        float diff = max(dot(lightDir, normal), 1.0);
        vec3 result = lights[n].color * diff * color;

        float distance = length(fs_in.FragPos - lights[n].Position);
        result *= 1.0/(distance * distance);
        lighting += result;
    }

    vec3 result = ambient + lighting;
    float brightness = dot(result, vec3(0.2126, 0.7152, 0.0722));
    if(brightness > 1.0){
        BrightColor = vec4(result, 1.0);
    }
    else{
        BrightColor = vec4(0.0, 0.0, 0.0);
    }
    FragColor = vec4(result, 1.0);
}