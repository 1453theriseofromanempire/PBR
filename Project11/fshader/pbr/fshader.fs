# version 330 core
out vec4 FragColor;

in vec2 TexCoords;
in vec3 WorldPos;
in vec3 Normal;

uniform vec3 albedo;
uniform float metallic;
uniform float roughness;
uniform float ao;

uniform vec3 lightPositions[4];
uniform vec3 lightColors[4];

uniform vec3 camPos;

const float PI = 3.14159265359;

float DistributionGGX(vec3 N, vec3 H, float roughness){
    float a = roughness * roughness;
    float a2 = a * a;
    float NH = pow(max(dot(N, H), 0), 2);
    float x = pow((NH * (a2 - 1) + 1), 2);

    return a2 / (PI * x);
}

float GeometrySchlickGGX(float V, float alpha){
    float nom = V;
    float denom = V * (1 - alpha) + alpha;

    return nom / denom;
}

float GeometrySmith(vec3 N, vec3 V, vec3 L, float alpha){
    float NdotV = max(dot(N, V), 0.0);
    float NdotL = max(dot(N, L), 0.0);
    float ggx1 = GeometrySchlickGGX(NdotV, alpha);
    float ggx2 = GeometrySchlickGGX(NdotL, alpha);

    return ggx1 * ggx2;
}

vec3 fresnelSchlick(float cosTheta, vec3 F0){
    return F0 + (1.0 - F0) * pow(clamp(1.0 - cosTheta, 0.0, 1.0), 5.0);
}

void main(){
    vec3 N = normalize(Normal);
    vec3 V = normalize(camPos - WorldPos);

    vec3 F0 = vec3(0.04f);
    F0 = mix(F0, albedo, metallic);
    vec3 Lo = vec3(0.0);
    for(int i = 0; i < 4; i++){
        vec3 L = normalize(lightPositions[i] - WorldPos);
        vec3 H = normalize(L + V);
        float dis = length(lightPositions[i] - WorldPos);
        float attenuation = 1 / (dis * dis);
        vec3 radiance = lightColors[i] * attenuation;

        float NDF = DistributionGGX(N, H, roughness);

        float alpha = pow((roughness + 1), 2) / 8.0;
        float G = GeometrySmith(N, V, L, alpha);

        vec3 F = fresnelSchlick(clamp(dot(H, V), 0.0, 1.0), F0);

        vec3 numerator = NDF * G * F;
        float denominator = 4.0 * max(dot(N, V), 0.0) * max(dot(N, L), 0.0) + 0.0001;
        vec3 specular = numerator / denominator;

        vec3 Kd = vec3(1.0) - F;
        Kd *= 1 - metallic;

        float NdotL = max(dot(N, L), 0.0);        
        Lo += (Kd * albedo / PI + specular) * radiance * NdotL; 

    }
    vec3 ambient = vec3(0.03) * albedo * ao;

    vec3 color = ambient + Lo;
    color = color / (color + vec3(1.0));

    color = pow(color, vec3(1.0/2.2)); 

    FragColor = vec4(color, 1.0);
}