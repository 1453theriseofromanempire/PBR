#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;

out VS_OUT {
    vec3 Normal;
} vs_out;

void main()
{
    vs_out.Normal = aNormal;
    gl_Position = vec4(aPos, 1.0); 
}