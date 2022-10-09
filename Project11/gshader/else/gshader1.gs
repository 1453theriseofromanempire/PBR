#version 330 core
layout (triangles) in;
layout (line_strip, max_vertices = 6) out;

in VS_OUT {
    vec3 Normal;
} gs_in[];

const float length = 0.2;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

void drawline(int num){
    normalize(gs_in[num].Normal);
    gl_Position = projection * view * model * gl_in[num].gl_Position;
    EmitVertex();
    gl_Position = projection * view * model * vec4(gl_in[num].gl_Position + vec4(gs_in[num].Normal, 0.0) * length);
    EmitVertex();
    EndPrimitive();
}

void main() {    
    drawline(0);
    drawline(1);
    drawline(2);
}