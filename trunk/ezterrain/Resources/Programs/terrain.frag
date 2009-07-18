//#version 130
#extension GL_EXT_gpu_shader4 : enable

uniform sampler2D noise;

varying in vec2 texCoord;

void main()
{
	gl_FragColor = texture2D(noise, texCoord);
}
