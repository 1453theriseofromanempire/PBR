#version 330 core
layout (location = 0) in vec2 aPos;
layout (location = 1) in vec3 aColor;
layout (location = 2) in vec2 offset;

out vec3 color;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

void main()
{
    color = aColor;
    vec2 pos = aPos * (gl_InstanceID / 100.0);
    gl_Position = projection * view * model * vec4(pos + offset, 0.0, 1.0); 
}