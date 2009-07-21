//#version 130
#extension GL_EXT_gpu_shader4 : enable

uniform float texScale;
uniform float outer;

in vec4 glVertex;

varying out vec2 texCoord;
varying out float level;

void main()
{
	level = outer + float(gl_InstanceID);
	float levelScale = pow(2.0, level);

	texCoord = glVertex.xy * texScale + 0.5;

	vec4 vertex = glVertex;
	vertex.xy = vertex.xy * levelScale;

	gl_Position = gl_ModelViewProjectionMatrix * vertex;
}
