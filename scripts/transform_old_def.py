#!/usr/bin/python3

# Get Value Regex (?<= = )(.+)(?=;)
# Get Option Regex (\[ConfigurationOption.+\]\n.+;)
# Get Assignment Regex ( = .+;)
# Get Position Regex \[ConfigurationOption\(".+",
import sys
import re

def main(filename):
	with (open(filename)) as f:
		content = f.read()
		m = re.findall(r'(\[ConfigurationOption.+\]\n.+;)', content)
		for x in m:
			value = re.search(r'(?<= = )(.+)(?=;)', x).group(0)
			header = re.search(r'(\[ConfigurationOption.+\])', x).group(0)
			print(header)
			pos = re.search(r'\[ConfigurationOption\("[a-zA-Z0-9_#]+",', header).end()
			new_header = header[:pos]+value+","+header[pos:]
			sub = re.search(r'( = .+;)', x).group(0)
			y = x.replace(sub, "")
			new_option = y.replace(header, new_header)
			content = content.replace(x, new_option)
		with open(''.join(['./out/', filename]), 'w+') as f:
			print(content.replace('public', '').replace('class', 'interface'), file=f)
if __name__ == "__main__":
    main(sys.argv[1])