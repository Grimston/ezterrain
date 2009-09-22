//#version 130
#extension GL_EXT_gpu_shader4 : enable

uniform float texScale;
uniform float outer;
uniform vec2 eyeOffset;
uniform sampler2DArray heightMaps;

in vec4 glVertex;

varying out vec2 texCoord;
varying out float level;

void main()
{
	level = outer + float(gl_InstanceID);
	float levelScale = pow(2.0, level);

	texCoord = (glVertex.xy * texScale + 0.5) / 2.0 + 0.25;

	vec4 vertex = glVertex;
	vertex.xy = vertex.xy * levelScale + eyeOffset;
	vertex.z = texture2DArray(heightMaps, vec3(texCoord, level)).r;

	gl_Position = gl_ModelViewProjectionMatrix * vertex;
}
