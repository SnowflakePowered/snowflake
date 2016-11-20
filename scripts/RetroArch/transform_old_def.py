#!/usr/bin/python3

# Get Value Regex (?<= = )(.+)(?=;)
# Get Option Regex (\[ConfigurationOption.+\]\n.+;)
# Get Assignment Regex ( = .+;)
# Get Position Regex \[ConfigurationOption\(".+",
import sys
import re
import os

def main(filename):
	with (open(filename)) as f:
		content = f.read()
		print(filename)
		sname, dname = re.search(r'(?:\s.+base\s?\()("[A-Za-z]+")(?:, )("[A-Za-z\s]+")', content).groups()
		content = re.sub(r'\s+public [A-Za-z]+\(\) : base\s?\("[A-Za-z]+", "[\sA-Za-z]+"\)$\s+\{$\s+}', "", content, flags=re.MULTILINE)
		content = re.sub(r'(?:\spublic class )([A-Za-z]+) : (ConfigurationSection)', r'[ConfigurationSection('+sname+r', '+dname+r')]\r\npublic interface \1 : IConfigurationSection<\1>', content)
		m = re.findall(r'(\[ConfigurationOption.+\]\n.+;)', content)
		for x in m:
			value = re.search(r'(?<= = )(.+)(?=;)', x).group(0)
			header = re.search(r'(\[ConfigurationOption.+\])', x).group(0)
			pos = re.search(r'\[ConfigurationOption\("[~a-zA-Z0-9_#]+",', header).end()
			new_header = header[:pos]+value+","+header[pos:]
			sub = re.search(r'( = .+;)', x).group(0)
			y = x.replace(sub, "")
			new_option = y.replace(header, new_header)
			content = content.replace(x, new_option)
		with open(''.join(['./out/', filename]), 'w+') as f:
			print(content.replace('public ', '').replace('interface', ' public interface'), file=f)
if __name__ == "__main__":
	files = [f for f in os.listdir('.') if os.path.isfile(f) and f.endswith('cs')]
	for f in files:
		main(f)